using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Shop_Mvc.Models
{
    public class Product
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string? OnlyInNovus { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public string? Promo { get; set; }
        public string? Country { get; set; }
        public string? Method { get; set; }
        public string? Basis { get; set; }
        public string? Temperature { get; set; }
        public string? Composition { get; set; }
        public string? Description { get; set; } 
        public string? Expiration_date { get; set; }
        public string? Storage_conditions { get; set; }
        public string? Caloric { get; set; }
        public string? Carbohydrate { get; set; }
        public string? Fat { get; set; }
        public string? Protein { get; set; }
        public string? Allergen { get; set; }
        public string? Refuel { get; set; }
        public string? Quantity_in_package { get; set; }
        public string? Energy_value { get; set; }
        public string? Sort { get; set; }
        public string? Features { get; set; }
        public string? For_children_with { get; set; }
        public string? Microelements { get; set; }
        public string? Vitamins { get; set; }
        public string? Type_of_cheese { get; set; }
        public string? Type_of_sausage { get; set; }
        public string? Method_of_processing { get; set; }
        public string? By_composition { get; set; }
        public string? Quantity_box_package { get; set; }
        public string? Diaper_size { get; set; }
        public string? Alcohol { get; set; }
        public string? Temperature_wine_serving { get; set; }
        public string? Region { get; set; }
        public string? Wine_classification { get; set; }
        public string? Aging_in_barrel { get; set; }
        public string? Package_volume { get; set; }
        public double? Price { get; set; }
        public byte[]? Image { get; set; }
    }
}
