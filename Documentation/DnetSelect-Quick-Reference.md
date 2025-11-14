# DnetSelect Quick Reference Card

## Minimal Setup (Copy & Paste Ready)

### 1. Basic String Select
```razor
<DnetSelect TValue="string" TItem="string"
            @bind-Value="SelectedValue"
            Items="StringList"
            DisplayValueConverter="(item) => item"
            ValueConverter="(item) => item"
            PlaceHolder="Choose option" />
```

### 2. Object Select by ID
```razor
<DnetSelect TValue="int" TItem="MyObject"
            @bind-Value="SelectedId" 
            Items="ObjectList"
            DisplayValueConverter="(obj) => obj.Name"
            ValueConverter="(obj) => obj.Id"
            PlaceHolder="Select item" />
```

### 3. Multi-Select
```razor
<DnetSelect TValue="string" TItem="MyObject"
            Items="ObjectList"
            SelectedItems="SelectedObjects"
            DisplayValueConverter="(obj) => obj.Name"
            MultiSelect="true"
            IsSelectedFn="(obj1, obj2) => obj1.Id == obj2.Id"
            OnSelectionChanged="@((List<MyObject> items) => HandleSelection(items))"
            PlaceHolder="Select multiple" />
```

## Required Parameters by Scenario

### Single Select (Always Required)
- `TValue="TypeOfBoundValue"`
- `TItem="TypeOfSourceItem"`
- `Items="List<TItem>"`
- `DisplayValueConverter="(item) => item.DisplayProperty"`
- `ValueConverter="(item) => item.ValueProperty"`

### Multi-Select (Always Required)
- Same as above PLUS:
- `MultiSelect="true"`
- `IsSelectedFn="(item1, item2) => item1.Id == item2.Id"`
- `SelectedItems="List<TItem>"`
- `OnSelectionChanged="EventCallback"`

## Common Parameter Patterns

```razor
<!-- Form Integration -->
@bind-Value="model.PropertyId"
ValueExpression="@(() => model.PropertyId)"

<!-- Sizing -->
Width="300px" Height="200px" ItemHeight="40"

<!-- Behavior -->
Disabled="isDisabled" PlaceHolder="Helpful text"

<!-- Events -->
OnItemSelected="@((TItem item) => HandleItemSelected(item))"
OnSelectionChanged="@((List<TItem> items) => HandleMultiSelection(items))"

<!-- Custom Display -->
ItemChildContent="@((item) => @<div><strong>@item.Name</strong><br/><small>@item.Details</small></div>)"
```

## Troubleshooting Checklist

✅ **TValue matches the type of your bound property**  
✅ **TItem matches the type of items in your Items collection**  
✅ **DisplayValueConverter returns a string**  
✅ **ValueConverter returns TValue type**  
✅ **For multi-select: IsSelectedFn compares items correctly**  
✅ **Items collection is not null/empty**  
✅ **For form validation: ValueExpression is provided**

## Quick Copy Templates

### Form Field Integration
```razor
<DnetFormField Label="Category" IsRequired="true">
    <ChildContent>
        <DnetSelect TValue="int" TItem="Category"
                    @bind-Value="model.CategoryId"
                    ValueExpression="@(() => model.CategoryId)"
                    Items="Categories"
                    DisplayValueConverter="(cat) => cat.Name"
                    ValueConverter="(cat) => cat.Id"
                    PlaceHolder="Select category" />
    </ChildContent>
    <ErrorContent>
        <ValidationMessage For="@(() => model.CategoryId)" />
    </ErrorContent>
</DnetFormField>
```

### Cascading Dropdowns
```razor
<!-- Parent -->
<DnetSelect @bind-Value="ParentId" OnItemSelected="@((Parent p) => LoadChildren(p.Id))" ... />

<!-- Child (depends on parent) -->
<DnetSelect @bind-Value="ChildId" Items="FilteredChildren" Disabled="@(ParentId == 0)" ... />
```
