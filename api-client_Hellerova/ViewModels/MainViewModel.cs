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
using System.Windows;
using api_client_Hellerova.Models;
using Newtonsoft.Json;

namespace api_client_Hellerova.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("https://localhost:44320/");
        private HttpClient _client;

        private string _response ="";
        private string _statusMessage;
        private string _statusMessage2;
        private Films _beruska;
        private Films _newFilm;
        private Zanrs _newZanr;
        private string _response2;
        private string _response3;
        private string _response4;
        private Zanrs _beruska2;
        private Films[] _filmy;
        private Zanrs[] _zanry;
        private Zanrs[] _zanrii;
        private Films[] _filmyy;
        public string _hledani;
        public string _hledani2;
        public Films[] _filmyvzanru;
        private Films _editedFilm;
        private Zanrs _editedZanr;
        private ObservableCollection<Films> _filmyvzanru2 = new ObservableCollection<Films>();
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
                    try
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
                    catch {};
                });

            SaveChangesCommand = new RelayCommand(
                async () =>{
                    try
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        await _client.PutAsJsonAsync("api/Films/ZmenitAtributy?id=" + EditedFilm.filmid, EditedFilm);
                        if (response.IsSuccessStatusCode)
                        {
                            StatusMessage2 = "Změny byly uloženy.";
                        }
                        else
                        {
                            StatusMessage2 = "Změny se nepodařilo uložit.";
                        }
                    }
                    catch
                    {
                        StatusMessage2 = "Změny se nepodařilo uložit.";
                    }
                });


            SaveChanges2Command = new RelayCommand(
                async () =>{

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.PutAsJsonAsync("api/Zanrs/ZmenitAtributy?id=" + EditedZanr.zanrid, EditedZanr);
                    if (response.IsSuccessStatusCode)
                    {
                        StatusMessage = "Změny byly uloženy.";
                    }
                    else
                    {
                        StatusMessage = "Změny se nepodařilo uložit.";
                    }
                }
                );

            AddNewFilmCommand = new RelayCommand(
                async () =>
                {
                    try
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await _client.GetAsync("api/Films/VypsatFilmy");
                        Films film = new Films() { name = NewFilm.name, zanrid = NewFilm.zanrid };
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                response = await _client.PostAsJsonAsync("api/Films/PridatNovyFilm", film);
                                if (!response.IsSuccessStatusCode)
                                {
                                    //Filmy.Clear();
                                    StatusMessage2 = "Film se nepodařilo přidat.";
                                }
                                else
                                {
                                    Filmy.Add(film);
                                    StatusMessage2 = "Film byl přidán.";
                                }

                            }
                            catch
                            {
                                //MessageBox.Show("Chyba", response.StatusCode.ToString());
                                Filmy.Clear();
                                StatusMessage2 = "Film se nepodařilo přidat.";
                            }

                        }
                    }
                    catch
                    {
                        StatusMessage2 = "Chyba.";
                    }
                    
                }
                );

            AddNewZanrCommand = new RelayCommand(
                async () => {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("api/Zanrs/VypsatZanry");
                    Zanrs zanr = new Zanrs() { name = NewZanr.name };
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            response = await _client.PostAsJsonAsync("api/Zanrs/PridatNovyZanr", zanr);
                            Zanry.Add(zanr);
                            StatusMessage = "Žánr byl přidán";
                        }
                        catch
                        {
                            //MessageBox.Show("Chyba", response.StatusCode.ToString());
                            Filmy.Clear();
                            StatusMessage = "Žánr se nepodařilo přidat.";
                        }

                    }
                }
                );
            Vyhledat = new ParametrizedRelayCommand<Zanrs>(
               async (value) =>
               {
                   if (Hledani == null)
                   {
                       Hledani = "";
                   }
                   HttpResponseMessage response4 = new HttpResponseMessage();
                   response4 = await _client.GetAsync("api/Zanrs/VypsatZanry");
                   Response4 = await response4.Content.ReadAsStringAsync();
                   _zanrii = JsonConvert.DeserializeObject<Zanrs[]>(Response4);
                   Zanricky = new List<Zanrs>(_zanrii).Where(x => x.name.Contains(Hledani)).ToList();
                   Zanry = new ObservableCollection<Zanrs>(Zanricky);
               },
               (parameter) =>
               {
                   return true;
               });

           Vyhledat2 = new ParametrizedRelayCommand<Zanrs>(
               async (value) =>
               {
                   if (Hledani2 == null)
                   {
                       Hledani2 = "";
                   }
                   HttpResponseMessage response4 = new HttpResponseMessage();
                   response4 = await _client.GetAsync("api/Films/VypsatFilmy");
                   Response4 = await response4.Content.ReadAsStringAsync();
                   _filmyy = JsonConvert.DeserializeObject<Films[]>(Response4);
                   Filmicky = new List<Films>(_filmyy).Where(x => x.name.Contains(Hledani2)).ToList();
                   Filmy = new ObservableCollection<Films>(Filmicky);
               },
               (parameter) =>
               {
                   return true;
               });

            FilmyVZanru = new RelayCommand(
                async () => {
                    HttpResponseMessage response3 = new HttpResponseMessage();
                    response3 = await _client.GetAsync("api/Zanrs/VypsatFilmyZanru?id=" + Beruska2.zanrid);
                    Response3 = await response3.Content.ReadAsStringAsync();
                    _filmyvzanru = JsonConvert.DeserializeObject<Films[]>(Response3);
                    FilmikyVZanru = new ObservableCollection<Films>(_filmyvzanru);
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
                     StatusMessage2 = "Film byl odebrán.";
                 }
                 else
                 {
                     Filmy.Clear();
                     StatusMessage2 = "Film se nepodařilo odebrat.";
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
                     StatusMessage = "Žánr byl odebrán..";
                 }
                 else
                 {
                     Zanry.Clear();
                     StatusMessage = "Položku se nepovedlo odebrat.";
                 }
             },
             (parameter) => { return Beruska2 == null ? false : true; }
             );

            FilterCommand = new ParametrizedRelayCommand<string>(
             async (value) =>
             {
                 HttpResponseMessage response = new HttpResponseMessage();
                 response = await _client.GetAsync("api/Zanrs/Search?search=" + value);
                 //List<Films> FilteredCollectionOfFilms = new List<Films>();
                 //FilteredCollectionOfFilms = response;
                 if (response.IsSuccessStatusCode)
                 {
                     //Zanry = new ObservableCollection<Zanrs>().Where(x => x.name.Contains(value)).();
                     //FilteredCollectionOfFilms = FilteredCollectionOfFilms.Where(x => x.name.Contains(value)).ToList();
                     Response = await response.Content.ReadAsStringAsync();
                     //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response);
                     _zanry = JsonConvert.DeserializeObject<Zanrs[]>(Response);
                     //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                     Zanry = new ObservableCollection<Zanrs>(_zanry);
                 }
                 else
                 {
                     Zanry.Clear();
                 }
             },
             (parameter) => { return true; }
             );

            FilterCommand2 = new ParametrizedRelayCommand<string>(
             async (value) =>
             {
                 HttpResponseMessage response = new HttpResponseMessage();
                 response = await _client.GetAsync("api/Films/Search?search=" + value);
                 //List<Films> FilteredCollectionOfFilms = new List<Films>();
                 //FilteredCollectionOfFilms = response;
                 if (response.IsSuccessStatusCode)
                 {
                     //Zanry = new ObservableCollection<Zanrs>().Where(x => x.name.Contains(value)).();
                     //FilteredCollectionOfFilms = FilteredCollectionOfFilms.Where(x => x.name.Contains(value)).ToList();
                     Response = await response.Content.ReadAsStringAsync();
                     //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response);
                     _filmy = JsonConvert.DeserializeObject<Films[]>(Response);
                     //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                     Filmy = new ObservableCollection<Films>(_filmy);
                 }
                 else
                 {
                     Zanry.Clear();
                 }
             },
             (parameter) => { return true; }
             );


        }
        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public string Response2 { get { return _response2; } set { _response2 = value; NotifyPropertyChanged(); } }

        public string Response3 { get { return _response3; } set { _response3 = value; NotifyPropertyChanged(); } }
        public string StatusMessage { get { return _statusMessage; } set { _statusMessage = value; NotifyPropertyChanged(); } }
        public string StatusMessage2 { get { return _statusMessage2; } set { _statusMessage2 = value; NotifyPropertyChanged(); } }
        public string Response4 { get { return _response4; } set { _response4 = value; NotifyPropertyChanged(); } }
        public Films NewFilm { get { return _newFilm; } set { _newFilm = value; NotifyPropertyChanged(); } }
        public Zanrs NewZanr { get { return _newZanr; } set { _newZanr = value; NotifyPropertyChanged(); } }
        public string Hledani { get { return _hledani; } set { _hledani = value; NotifyPropertyChanged(); } }
        public string Hledani2 { get { return _hledani2; } set { _hledani2 = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Films> FilmikyVZanru { get { return _filmyvzanru2; } set { _filmyvzanru2 = value; NotifyPropertyChanged(); } }
        public List<Zanrs> Zanricky { get; set; }
        public List<Films> Filmicky { get; set; }
        public ObservableCollection<Films> Filmy { get { return _filmiky; } set { _filmiky = value; NotifyPropertyChanged(); RemoveCommand.RaiseCanExecureChanged(); } }

        public ObservableCollection<Zanrs> Zanry { get { return _zanryky; } set { _zanryky = value; NotifyPropertyChanged(); RemoveZanrCommand.RaiseCanExecureChanged(); } }
        public RelayCommand ReloadCommand { get; set; }
        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand SaveChanges2Command { get; set; }

        public RelayCommand FilmyVZanru { get; set; }
        public RelayCommand AddNewFilmCommand { get; set; }
        public RelayCommand AddNewZanrCommand { get; set; }
        public ParametrizedRelayCommand<Films> RemoveCommand { get; set; }

        public ParametrizedRelayCommand<Zanrs> Vyhledat { get; set; }
        public ParametrizedRelayCommand<Zanrs> Vyhledat2 { get; set; }
        public ParametrizedRelayCommand<string> FilterCommand { get; set; }
        public ParametrizedRelayCommand<string> FilterCommand2 { get; set; }

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
