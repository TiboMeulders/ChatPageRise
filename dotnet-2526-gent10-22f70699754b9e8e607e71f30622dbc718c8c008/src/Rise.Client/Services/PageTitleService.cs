using System;

namespace Rise.Client.Services{
    public class PageTitleService{
        private string _pageTitle = "Homepage";
        
        public string PageTitle{
            get => _pageTitle;
            set{
                _pageTitle = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}