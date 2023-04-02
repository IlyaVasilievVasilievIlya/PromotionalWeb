using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PromoWeb.Context.Entities;

namespace PromoWeb.Context
{
    public static class DbSeeder
    {
        private static IServiceScope ServiceScope(IServiceProvider serviceProvider) => serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
        private static MainDbContext DbContext(IServiceProvider serviceProvider) => ServiceScope(serviceProvider).ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();

        public static void Execute(IServiceProvider serviceProvider, bool addDemoData)
        {
            using var scope = ServiceScope(serviceProvider);
            ArgumentNullException.ThrowIfNull(scope);

            if (addDemoData)
            {
                Task.Run(async () =>
                {
                    await ConfigureDemoData(serviceProvider);
                });
            }
        }

        private static async Task ConfigureDemoData(IServiceProvider serviceProvider)
        {
            await AddApp(serviceProvider);
        }

        

        private static async Task AddApp(IServiceProvider serviceProvider)
        {
            await using var context = DbContext(serviceProvider);

            if (context.Contacts.Any() || context.Sections.Any() || context.Questions.Any())
                return;

            var sections = new Section[]
            {
                new Section { SectionName = "General Info" },
                new Section { SectionName = "Functions" },
                new Section { SectionName = "FAQ" },
                new Section { SectionName = "Advantages" },
                new Section { SectionName = "Guides" },
                new Section { SectionName = "Download links" }
            };
            context.Sections.AddRange(sections);

            var links = new Link[]
            {
                new Link { LinkText = "https://play.google.com/store/apps/details?id=com.smartshopperteam.monoscrobbler&referrer=mono-scrobbler%2F", Description = "Download in GooglePlay",
                            Section = sections[5] },
                new Link { LinkText = "https://apps.apple.com/RU/app/id13715657962341", Description = "Download in AppStore",
                            Section = sections[5]},
                new Link { LinkText = "...", Description = "Download in NameStore",
                            Section = sections[5]}
            };
            context.Links.AddRange(links);


            var contacts = new Contact[]
            {
                new Contact { ContactOwner = "Support Service", Email = "monoteam@tst.com", Phone = "803-355-35-35" },
                new Contact { ContactOwner = "Site administration", Email = "admin1@tst.com"},
                new Contact { ContactOwner = "Developer", Email = "monoteam@main.com", Phone = "804-355-35-35", WebSite = "MonoTeam.ru",
                              Address = "060-844, Russia, Voronezh, 1 Lenin str." }
            };
            context.Contacts.AddRange(contacts);


            var appinfos = new AppInfo[]
            {
                new AppInfo { Section = sections[0], TextTitle = "Mono Scrobbler",
                            Text = "Multifunctional music tracker with beautiful animation" },
                new AppInfo { Section = sections[1], TextTitle = "Scrobble on your device", Text = "Supports phones, TVs, tablets and Android desktops including Windows 11" },
                new AppInfo { Section = sections[1], TextTitle = "Flexible editing", Text = "Edit or delete existings scrobbles. Remembers edits, fix metadata such as \"Remastered\" or" +
                                                                                             "your own patterns with regesx edits, block artists, tracks etc and auto skip or mute when they play" },
                new AppInfo { Section = sections[1], TextTitle = "Lorem Ipsum",
                                                                            Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                                                            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
                                                                            " Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                                                                            "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in " +
                                                                            "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla " +
                                                                            "pariatur. Excepteur sint occaecat cupidatat non proident, sunt in " +
                                                                            "culpa qui officia deserunt mollit anim id est laborum.\r\n Velit scelerisque in dictum non consectetur a. " +
                                                                            "Malesuada fames ac turpis egestas maecenas pharetra convallis posuere. Turpis in eu mi bibendum. " +
                                                                            "Gravida dictum fusce ut placerat orci nulla pellentesque dignissim enim. Id eu nisl nunc mi. " +
                                                                            "Vulputate enim nulla aliquet porttitor. Amet consectetur adipiscing elit ut. Eget mauris pharetra et ultrices. " +
                                                                            "Nam libero justo laoreet sit amet." },
                new AppInfo { Section = sections[2], TextTitle = "What services does Mono Scrobbler support?", Text = "Mono Scrobbler supports LastFM, LibreFM and Listenbrainz" },
                new AppInfo { Section = sections[2], TextTitle = "What streaming services does Mono Scrobbler support?",
                                                                            Text = "Mono Scrobbler supports services that send metadata about the music the user has listened to. These services are the majority of those on the market." }
            };
            context.AppInfos.AddRange(appinfos);

            context.SaveChanges();
        }
    }
}
