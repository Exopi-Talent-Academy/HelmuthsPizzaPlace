namespace PizzaPlace.Test.TestExtensions;

public static class FakeTimeProviderExtensions
{
    public static void PassTimeInMinuteIntervals(this FakeTimeProvider fakeTimeProvider, int minutesToPass)
    {
        while (minutesToPass-- > 0)
        {
            fakeTimeProvider.Advance(TimeSpan.FromMinutes(1));
        }
    }

    public static async Task PassTimeInMinuteIntervalsAsync(
        this FakeTimeProvider fakeTimeProvider, int minutesToPass)
    {
        while (minutesToPass-- > 0)
        {
            fakeTimeProvider.Advance(TimeSpan.FromMinutes(1));

            // Allow pending Task.Delay continuations to resume.
            await Task.Yield();
        }
    }
}
