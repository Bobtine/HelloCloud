using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloCloud.Models
{
    public partial class Produit
    {
        [Key]
        public int IdProduit { get; set; }
        [StringLength(150)]
        public string NomProduit { get; set; } = null!;
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Prix { get; set; }
        public int? IdCategorie { get; set; }

        [ForeignKey("IdCategorie")]
        [InverseProperty("Produits")]
        public virtual Category? IdCategorieNavigation { get; set; }
    }
}
