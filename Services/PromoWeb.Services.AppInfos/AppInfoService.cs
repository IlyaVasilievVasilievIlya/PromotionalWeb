using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Common.Exceptions;
using PromoWeb.Common.Validator;
using PromoWeb.Context;
using PromoWeb.Context.Entities;

namespace PromoWeb.Services.AppInfos
{
    public class AppInfoService : IAppInfoService
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;
        private readonly IMapper mapper;
        private readonly IModelValidator<AddAppInfoModel> addAppInfoModelValidator;
        private readonly IModelValidator<UpdateAppInfoModel> updateAppInfoModelValidator;

        public AppInfoService(
            IDbContextFactory<MainDbContext> contextFactory,
            IMapper mapper,
            IModelValidator<AddAppInfoModel> addAppInfoModelValidator,
            IModelValidator<UpdateAppInfoModel> updateAppInfoModelValidator
            )
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
            this.addAppInfoModelValidator = addAppInfoModelValidator;
            this.updateAppInfoModelValidator = updateAppInfoModelValidator;
        }

        public async Task<IEnumerable<AppInfoModel>> GetAppInfos(int offset = 0, int limit = 10)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var appinfos = context
                .AppInfos
                .Include(x => x.Section) //в модели просто имя отобразится
                .AsQueryable();

            appinfos = appinfos
                .Skip(Math.Max(offset, 0))
                .Take(Math.Max(0, Math.Min(limit, 1000)));

            var data = (await appinfos.ToListAsync()).Select(info => mapper.Map<AppInfoModel>(info));

            return data;
        }

        public async Task<AppInfoModel> GetAppInfo(int id)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var info = await context.AppInfos.Include(x => x.Section)
                                        .FirstOrDefaultAsync(x => x.Id.Equals(id));

            var data = mapper.Map<AppInfoModel>(info);

            return data;
        }

        public async Task<AppInfoModel> AddAppInfo(AddAppInfoModel model)
        {
            addAppInfoModelValidator.Check(model);

            using var context = await contextFactory.CreateDbContextAsync();

            var info = mapper.Map<AppInfo>(model);
            await context.AppInfos.AddAsync(info);
            context.SaveChanges();

            return mapper.Map<AppInfoModel>(info);
        }

        public async Task UpdateAppInfo(int infoId, UpdateAppInfoModel model)
        {
            updateAppInfoModelValidator.Check(model);

            using var context = await contextFactory.CreateDbContextAsync();

            var info = await context.AppInfos.FirstOrDefaultAsync(x => x.Id.Equals(infoId));

            ProcessException.ThrowIf(() => info is null, $"The appinfo (id: {infoId}) was not found");
            
            info = mapper.Map(model, info);

            context.AppInfos.Update(info);
            context.SaveChanges();
        }

        public async Task DeleteAppInfo(int infoId)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var info = await context.AppInfos.FirstOrDefaultAsync(x => x.Id.Equals(infoId))
                ?? throw new ProcessException($"The appInfo (id: {infoId}) was not found");

            context.Remove(info); //restrictы ведут к исключениям
            context.SaveChanges();
        }
    }
}
