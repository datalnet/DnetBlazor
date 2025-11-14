# DnetSelect Component Usage Guide for LLM Integration

## Overview
The `DnetSelect` is a feature-rich Blazor dropdown/select component designed for both single and multi-selection scenarios. It provides advanced functionality including form validation, custom rendering, filtering, and overlay positioning.

## Component Declaration
```csharp
@using Dnet.Blazor.Components.Select
@using Dnet.Blazor.Components.Form

<DnetSelect TValue="TValue" TItem="TItem" @bind-Value="selectedValue" Items="itemsList" ... />
```

## Generic Type Parameters

### `TValue` (Required)
- **Purpose**: Represents the type of the value being bound to the component
- **Common Types**: `string`, `int`, `long`, `Guid`, custom types
- **Example**: `TValue="string"` for string values, `TValue="int"` for integer IDs

### `TItem` (Required) 
- **Purpose**: Represents the type of items in the data source collection
- **Usage**: Can be any class, record, or primitive type
- **Example**: `TItem="Person"` where Person is a custom class

## Essential Parameters

### Core Binding Parameters
```csharp
// Single Selection - Use @bind-Value for two-way binding
@bind-Value="selectedValue"                    // Two-way binding (recommended)
// OR manual binding
Value="selectedValue"                          // Current selected value
ValueChanged="@((TValue val) => OnValueChange(val))"  // Value change handler
ValueExpression="@(() => model.Property)"      // For validation binding

// Data Source
Items="itemsList"                              // List<TItem> - collection of items to display
```

### Display Configuration
```csharp
DisplayValueConverter="(item) => item.DisplayProperty"     // Required: How to display items
ValueConverter="(item) => item.ValueProperty"              // Required for single select: Extract value from item
PlaceHolder="Select an option"                             // Placeholder text when no selection
```

## Basic Usage Examples

### 1. Simple String Selection
```csharp
// Model
public string SelectedCountry { get; set; }
public List<string> Countries = new() { "USA", "Canada", "Mexico" };

// Component
<DnetSelect TValue="string" 
            TItem="string"
            @bind-Value="SelectedCountry"
            Items="Countries"
            DisplayValueConverter="(country) => country"
            ValueConverter="(country) => country"
            PlaceHolder="Select a country" />
```

### 2. Object Selection with Custom Properties
```csharp
// Model Classes
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public int SelectedPersonId { get; set; }
public List<Person> People = new();

// Component
<DnetSelect TValue="int"
            TItem="Person" 
            @bind-Value="SelectedPersonId"
            Items="People"
            DisplayValueConverter="(person) => person.Name"
            ValueConverter="(person) => person.Id"
            PlaceHolder="Select a person" />
```

### 3. Multi-Selection Mode
```csharp
// Model
public List<Person> SelectedPeople = new();
public List<Person> AllPeople = new();

// Component  
<DnetSelect TValue="string"
            TItem="Person"
            Value="@string.Empty"
            Items="AllPeople"
            SelectedItems="SelectedPeople"
            DisplayValueConverter="(person) => person.Name"
            MultiSelect="true"
            IsSelectedFn="(person1, person2) => person1.Id == person2.Id"
            OnSelectionChanged="@((List<Person> selected) => OnPeopleSelected(selected))"
            PlaceHolder="Select people" />
```

## Advanced Parameters

### Event Handlers
```csharp
OnItemSelected="@((TItem item) => HandleItemSelected(item))"        // Single item selected
OnSelectionChanged="@((List<TItem> items) => HandleSelectionChanged(items))"  // Multi-selection changed
```

### Visual Customization
```csharp
Width="300px"                                  // Dropdown trigger width
Height="200px"                                 // Dropdown panel height  
MinWidth="200px"                               // Minimum panel width
MaxWidth="500px"                              // Maximum panel width
MinHeight="100px"                             // Minimum panel height
MaxHeight="400px"                             // Maximum panel height
ItemHeight="40"                               // Height of each item in pixels
BorderRadius="5px"                            // Border radius for panel
MarginTop="5px"                               // Top margin of panel
```

### Behavior Configuration
```csharp
Disabled="false"                              // Enable/disable component
InputTextToUpper="true"                       // Convert input text to uppercase
ItemAutoSelection="true"                      // Auto-select first matching item
DisabledClearButton="false"                   // Show/hide clear button
ConfirmButtons="true"                         // Show confirm/cancel buttons in multi-select
IsRequired="true"                             // Mark as required field
```

### Custom Content Templates
```csharp
// Custom item display in dropdown
ItemChildContent="@((item) => 
    @<div>
        <strong>@item.Name</strong>
        <small>@item.Description</small>
    </div>
)"

// Custom item prefix (left side)
ItemPrefixContent="@((item) => 
    @<img src="@item.ImageUrl" alt="@item.Name" />
)"

// Custom item suffix (right side)  
ItemSufixContent="@((item) =>
    @<span class="badge">@item.Status</span>
)"

// Custom selected value display
ValueDisplayContent="@((item) =>
    @<div class="selected-item">
        <img src="@item.Avatar" />
        <span>@item.Name</span>
    </div>
)"
```

### Advanced Features
```csharp
SupportTextValueConverter="(item) => item.Description"  // Secondary text under main display
OverlayPanelClass="custom-panel-style"                 // CSS class for dropdown panel
ResponsiveLabel="Select Option"                         // Label for responsive/mobile view
```

## Form Integration

### With Form Validation
```csharp
<EditForm Model="formModel" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    
    <DnetFormField Label="Category" IsRequired="true">
        <ChildContent>
            <DnetSelect TValue="int"
                        TItem="Category"
                        @bind-Value="formModel.CategoryId" 
                        ValueExpression="@(() => formModel.CategoryId)"
                        Items="Categories"
                        DisplayValueConverter="(cat) => cat.Name"
                        ValueConverter="(cat) => cat.Id"
                        PlaceHolder="Select category" />
        </ChildContent>
        <ErrorContent>
            <ValidationMessage For="@(() => formModel.CategoryId)" />
        </ErrorContent>
        <HintContent>
            Choose the appropriate category for this item
        </HintContent>
    </DnetFormField>
</EditForm>
```

## Common Patterns

### 1. Dependent/Cascading Dropdowns
```csharp
// First dropdown changes, second dropdown updates its items
<DnetSelect TValue="int"
            TItem="Category"
            @bind-Value="SelectedCategoryId"
            Items="Categories"
            DisplayValueConverter="(cat) => cat.Name"
            ValueConverter="(cat) => cat.Id"
            OnItemSelected="@((Category cat) => LoadSubcategories(cat.Id))"
            PlaceHolder="Select category" />

<DnetSelect TValue="int"
            TItem="Subcategory" 
            @bind-Value="SelectedSubcategoryId"
            Items="Subcategories"
            DisplayValueConverter="(sub) => sub.Name"
            ValueConverter="(sub) => sub.Id"
            Disabled="@(SelectedCategoryId == 0)"
            PlaceHolder="Select subcategory" />
```

### 2. Search/Filter Integration
```csharp
// Component handles filtering internally based on DisplayValueConverter
<DnetSelect TValue="string"
            TItem="Person"
            @bind-Value="SelectedPersonEmail"
            Items="FilteredPeople"
            DisplayValueConverter="(person) => $"{person.Name} ({person.Email})"
            ValueConverter="(person) => person.Email"
            PlaceHolder="Search people..." />
```

### 3. Custom Item Templates with Rich Content
```csharp
<DnetSelect TValue="int"
            TItem="Product"
            @bind-Value="SelectedProductId"
            Items="Products"
            DisplayValueConverter="(product) => product.Name"
            ValueConverter="(product) => product.Id"
            ItemChildContent="@((product) =>
                @<div class="product-item">
                    <div class="product-header">
                        <img src="@product.ThumbnailUrl" alt="@product.Name" class="product-thumb" />
                        <div class="product-info">
                            <div class="product-name">@product.Name</div>
                            <div class="product-description">@product.ShortDescription</div>
                        </div>
                    </div>
                    <div class="product-price">$@product.Price.ToString("F2")</div>
                </div>
            )"
            Height="400px"
            ItemHeight="80"
            PlaceHolder="Select a product" />
```

## Required vs Optional Parameters

### Always Required
- `TValue` and `TItem` type parameters
- `Items` - data source collection
- `DisplayValueConverter` - how to display items
- For single select: `ValueConverter` - how to extract value from item  
- For multi-select: `IsSelectedFn` - how to determine if items are equal

### Commonly Used Optional
- `PlaceHolder` - user guidance
- `@bind-Value` or (`Value` + `ValueChanged` + `ValueExpression`) - data binding
- `Width`, `Height` - sizing
- `Disabled` - state control
- `OnItemSelected`, `OnSelectionChanged` - event handling

### Advanced Optional
- Custom content templates (`ItemChildContent`, `ItemPrefixContent`, etc.)
- Size constraints (`MinWidth`, `MaxWidth`, etc.)
- Styling (`BorderRadius`, `OverlayPanelClass`)
- Behavior flags (`InputTextToUpper`, `ItemAutoSelection`, etc.)

## Error Handling & Validation

The component integrates with Blazor's validation system:
```csharp
// Model with validation attributes
public class FormModel
{
    [Required(ErrorMessage = "Please select a category")]
    public int CategoryId { get; set; }
    
    [MinLength(1, ErrorMessage = "Please select at least one item")]
    public List<int> SelectedItems { get; set; } = new();
}

// Component usage
<DnetSelect @bind-Value="model.CategoryId"
            ValueExpression="@(() => model.CategoryId)"
            ... />
```

## Performance Considerations

- Use `ItemHeight` parameter for large lists to enable virtualization
- Implement efficient `IsSelectedFn` for multi-select scenarios
- Consider `MaxHeight` to prevent extremely tall dropdowns
- Cache computed display values when using complex `DisplayValueConverter` functions

This guide covers the complete API surface and common usage patterns for the DnetSelect component, enabling effective integration in any Blazor application.
