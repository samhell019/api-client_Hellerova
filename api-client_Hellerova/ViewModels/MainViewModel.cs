using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        private Films _beruska;
        private string _response2;
        private Zanrs _beruska2;
        private Films[] _filmy;
        private Zanrs[] _zanry;
        private Films _editedFilm;
        private Zanrs _editedZanr;
        private ObservableCollection<Films> _filmiky = new ObservableCollection<Films>();
        private ObservableCollection<Zanrs> _zanryky = new ObservableCollection<Zanrs>();

        public Films EditedFilm
        {
            get { return _editedFilm; }
            set { _editedFilm = value; NotifyPropertyChanged(); }
        }

        public Zanrs EditedZanr
        {
            get { return _editedZanr; }
            set { _editedZanr = value; NotifyPropertyChanged(); }
        }

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

            SaveChangesCommand = new RelayCommand(
                async () => {
                    await _client.PutAsJsonAsync("api/Films/ZmenitAtributy?id=" + EditedFilm.filmid, EditedFilm);
                }
                );

            SaveChanges2Command = new RelayCommand(
                async () => {
                    await _client.PutAsJsonAsync("api/Zanrs/ZmenitAtributy?id=" + EditedZanr.zanrid, EditedZanr);
                }
                );

            RemoveCommand = new ParametrizedRelayCommand<Films>(
             async (value) =>
             {
                 HttpResponseMessage response = new HttpResponseMessage();
                 response = await _client.GetAsync("api/Films/VypsatFilmy");
                 if (response.IsSuccessStatusCode)
                 {
                     response = await _client.DeleteAsync($"api/Films/SmazatFilm?id={value.filmid}");
                     Filmy.Remove(value);
                 }
                 else
                 {
                     Filmy.Clear();
                 }
             },
             (parameter) => { return Beruska == null ? false : true; }
             );


            RemoveZanrCommand = new ParametrizedRelayCommand<Zanrs>(
             async (value) =>
             {
                 HttpResponseMessage response = new HttpResponseMessage();
                 response = await _client.GetAsync("api/Zanrs/VypsatZanry");
                 if (response.IsSuccessStatusCode)
                 {
                     response = await _client.DeleteAsync($"api/Zanrs/SmazatZanr?id={value.zanrid}");
                     Zanry.Remove(value);
                 }
                 else
                 {
                     Zanry.Clear();
                 }
             },
             (parameter) => { return Beruska2 == null ? false : true; }
             );

        }
        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public string Response2 { get { return _response2; } set { _response2 = value; NotifyPropertyChanged(); } }

        public ObservableCollection<Films> Filmy { get { return _filmiky; } set { _filmiky = value; NotifyPropertyChanged(); RemoveCommand.RaiseCanExecureChanged(); } }

        public ObservableCollection<Zanrs> Zanry { get { return _zanryky; } set { _zanryky = value; NotifyPropertyChanged(); RemoveZanrCommand.RaiseCanExecureChanged(); } }
        public RelayCommand ReloadCommand { get; set; }
        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand SaveChanges2Command { get; set; }
        public ParametrizedRelayCommand<Films> RemoveCommand { get; set; }
        public Films Beruska { get { return _beruska; } set { _beruska = value; NotifyPropertyChanged(); RemoveCommand.RaiseCanExecureChanged(); } }
        public ParametrizedRelayCommand<Zanrs> RemoveZanrCommand { get; set; }
        public Zanrs Beruska2 { get { return _beruska2; } set { _beruska2 = value; NotifyPropertyChanged(); RemoveZanrCommand.RaiseCanExecureChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
