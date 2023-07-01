using System.ComponentModel;

namespace Web.Models.Catalogs;

public class FeatureViewModel
{
    [DisplayName("Kurs Süresi : ")]
    public int Duration { get; set; }
}
