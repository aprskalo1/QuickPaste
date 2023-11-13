namespace QuickPaste.Mapping
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.FileStorage, DTO_s.FileStorageDTO>();
            CreateMap<DTO_s.FileStorageDTO, Models.FileStorage>();
        }
    }
}
