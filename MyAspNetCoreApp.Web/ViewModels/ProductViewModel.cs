

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Remote(action: "HasProductName", controller: "Products")]
        [StringLength(50,ErrorMessage ="İsim alanı en fazla 50 karakter içerebilir")]
        [Required (ErrorMessage ="İsim alanı boş olamaz")]
        public string Name { get; set; }

        //[RegularExpression(@"^[0-9]+(\.[0-9]{1,2})",
        // ErrorMessage = "Fiyat alanında noktadan sonra 2 basamak olmalıdır")]
        [Required(ErrorMessage = "Fiyat alanı boş olamaz")]
        public decimal? Price { get; set; }
        
        [Required(ErrorMessage = "Stok alanı boş olamaz")]
        [Range(1,200,ErrorMessage ="Stok sayısı 1 ile 200 arasında olmalıdır")]
        public int? Stock { get; set; }

        [StringLength(300,MinimumLength =10, ErrorMessage = "Açıklama en az 10 en fazla 300 karakter içerebilir")]
        [Required(ErrorMessage = "Açıklama alanı boş olamaz")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Renk alanı boş olamaz")]
        public string Color { get; set; }
        
        public bool IsPublish { get; set; }
        
        [Required(ErrorMessage = "Tarih alanı boş olamaz")]   
        public DateTime? PublishDate { get; set; }
        
        [Required(ErrorMessage = "Yayınlanma süresi boş olamaz")]
        public int? Expire { get; set; }

        //[Required]
        //[EmailAddress(ErrorMessage ="Email adresi uygun formatta değil")]
        //public string EmailAddress { get; set; }

    }
}
