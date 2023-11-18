using System;
using System.ComponentModel.DataAnnotations;

namespace Dnet.App.Shared.Infrastructure.Entities
{
    public class Person
    {
        public string PersonId { get; set; }

        public int? Index { get; set; }

        public bool IsActive { get; set; } = false;

        public string Balance { get; set; }

        public string Picture { get; set; }

        public int? Age { get; set; }

        public string EyeColor { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Company { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z0-9.-]+$", ErrorMessage = "Introduzca un email valido.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Registered { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Greeting { get; set; }

        public string FavoriteFruit { get; set; }

        public DateTime Birthday { get; set; }

        public double? Sales { get; set; }

        public double? Amount { get; set; }

        public string StartDate { get; set; }
    }
}
