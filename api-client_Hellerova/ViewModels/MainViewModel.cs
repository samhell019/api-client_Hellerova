using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using api_client_Hellerova.Models;
using Newtonsoft.Json;

namespace api_client_Hellerova.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("https://localhost:44320/");
        private HttpClient _client;

        private string _response;
        private string _response2;
        private Films[] _filmy;
        private Zanrs[] _zanry;
        private ObservableCollection<Films> _filmiky = new ObservableCollection<Films>();
        private ObservableCollection<Zanrs> _zanryky = new ObservableCollection<Zanrs>();

        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            ReloadCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpResponseMessage response2 = new HttpResponseMessage();
                    response = await _client.GetAsync("api/Films/VypsatFilmy");
                    response2 = await _client.GetAsync("api/Zanrs/VypsatZanry");
                    if (response.IsSuccessStatusCode && response2.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        Response2 = await response2.Content.ReadAsStringAsync();
                        //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response);
                        _filmy = JsonConvert.DeserializeObject<Films[]>(Response);
                        _zanry = JsonConvert.DeserializeObject<Zanrs[]>(Response2);
                        //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        Filmy = new ObservableCollection<Films>(_filmy);
                        Zanry = new ObservableCollection<Zanrs>(_zanry);
                    }
                    else
                    {
                        Response = "OOPS";
                        Filmy.Clear();
                        Zanry.Clear();
                    }
                }
                );
        }
        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public string Response2 { get { return _response2; } set { _response2 = value; NotifyPropertyChanged(); } }

        public ObservableCollection<Films> Filmy { get { return _filmiky; } set { _filmiky = value; NotifyPropertyChanged(); } }

        public ObservableCollection<Zanrs> Zanry { get { return _zanryky; } set { _zanryky = value; NotifyPropertyChanged(); } }
        public RelayCommand ReloadCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
