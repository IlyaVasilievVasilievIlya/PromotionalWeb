namespace PromoWeb.Services.Sections;

public interface ISectionService
{
    Task<IEnumerable<SectionModel>> GetSections(int offset = 0, int limit = 10);
    Task<SectionModel> GetSection(int sectionId);
    Task<SectionModel> AddSection(AddSectionModel model);
    Task UpdateSection(int id, UpdateSectionModel model);
    Task DeleteSection(int sectionId);
}