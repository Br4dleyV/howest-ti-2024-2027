using ReadingSummary.Api.Models;

namespace ReadingSummary.Api.Services
{
    public static class ReadingSummaryCalculator
    {
        public static Models.ReadingSummary Calculate(IEnumerable<TitleItem> titles)
        {
            int unread = 0;
            int finished = 0;

            foreach (TitleItem title in titles)
            {
                if (title.IsRead)
                    finished++;
                else
                    unread++;
            }

            return new Models.ReadingSummary(unread, finished);
        }
    }
}
