using Caliburn.Micro;

namespace Refracto
{
    public class CreateTimelineViewModel : Screen
    {
        string m_Name = "";

        public string Name
        {
            get => m_Name;
            set
            {
                if (Set(ref m_Name, value))
                {
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange(() => CanAccept);
                }
            }
        }

        public bool CanAccept => Name != "";

        public void Accept()
        {
            TryClose(true);
        }
    }
}
