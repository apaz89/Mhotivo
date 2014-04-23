﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Models
{
    [Table("ClassActivity")]
    public class ClassActivity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
    }

    public class DisplayClassActivityModel
    {

        public int ClassActivityId { get; set; }

        [Display(Name = "Año Académico")]
        public string AcademicYear { get; set; }

        [Display(Name = "Título")]
        public string DisplayName { get; set; }

        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Valor")]
        public string Value { get; set; }

    }

    public class ClassActivityEditModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Año Académico")]
        public int AcademicYearId { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Valor")]
        public double Value { get; set; }
    }

    public class ClassActivityRegisterModel
    {
        [Required]
        [Display(Name = "Año Académico")]
        public int AcademicYearId { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Valor")]
        public double Value { get; set; }
    }
}