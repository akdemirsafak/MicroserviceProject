using AutoMapper;
namespace Order.Application.Mapping
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy= new Lazy<IMapper>(()=>{
            var config=new MapperConfiguration(cfg=>{
                cfg.AddProfile<CustomMapping>();
            });
            return config.CreateMapper();
        }); //Parametre almayan Imapper'ı implemente etmiş bir class veriyoruz. 
        //Lazy ile Proje up olduğunda değil çağırıldığında initialize edilmesini sağlıyoruz
    
    public static IMapper Mapper=> lazy.Value; //Just get property 
    //When calling ObjectMapper.Mapper will Initialize.
    
    }
}