
namespace CharTracker.Model
{
    public class Locator
    {
        public Terminal Terminal { get; private set; }

        public Locator() { Terminal = Terminal.Instance; }
    }
}
