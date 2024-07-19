using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Extensions
{
    public static class RepositoryEventExtensions
    {
        public static IQueryable<Event> SearchByName(this IQueryable<Event> events, string searchByName)
        {
            if (string.IsNullOrWhiteSpace(searchByName)) return events;
            var lowerCaseName = searchByName.Trim().ToLower();

            return events.Where(e => e.Name.ToLower().Contains(lowerCaseName));
        }

        public static IQueryable<Event> FilterByDate(this IQueryable<Event> events, string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return events;

            return events.Where(e => e.Date.Contains(date));
        }

        public static IQueryable<Event> FilterByAdress(this IQueryable<Event> events, string adress)
        {
            if (string.IsNullOrWhiteSpace(adress)) return events;

            return events.Where(e => e.Adress.Contains(adress));
        }

        public static IQueryable<Event> FilterByCategory(this IQueryable<Event> events, int? category)
        {
            if (category is null) return events;

            return events.Where(e => e.Category.Id.Equals(category));
        }
    }
}
