using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloCloud.Models
{
    public partial class Category
    {
        public Category()
        {
            Produits = new HashSet<Produit>();
        }

        [Key]
        public int IdCategorie { get; set; }
        [StringLength(100)]
        public string NomCategorie { get; set; } = null!;

        [InverseProperty("IdCategorieNavigation")]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
