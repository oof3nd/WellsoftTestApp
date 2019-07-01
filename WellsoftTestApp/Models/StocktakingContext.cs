using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WellsoftTestApp.Models
{
    public class StocktakingContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public StocktakingContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }


    public class Company
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано наименование")]
        [Display(Name = "Наименование компании")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина строки должна быть от 5 до 200 символов")]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual List<Employee> Employees { get; set; }

    }

    public class Employee
    { 
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Неверно имя")]
        [Display(Name = "Имя сотрудника")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Длина строки должна быть от 5 до 200 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Неверно описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Неверный возраст, введите от 5 до 99")]
        [Range(5, 99, ErrorMessage = "Неверный возраст, введите от 5 до 99")]
        public int Age { get; set; }
        [Required]
        [Display(Name = "Дата добавление")]
        public DateTime DateAdd { get; set; } = DateTime.Now;
        [Required]
        [JsonIgnore]
        public int CompanyId { get; set; }
        [Display(Name = "Компания")]
        public virtual Company Company { get; set; }
    }


}
