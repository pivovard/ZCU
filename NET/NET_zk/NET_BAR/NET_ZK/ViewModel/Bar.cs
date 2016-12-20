using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET_ZK.Model;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls;

namespace NET_ZK.ViewModel
{
    [Serializable]
    public class Bar : NotifyBase
    {
        private ObservableCollection<Koktejl> koktejly;
        public ObservableCollection<Koktejl> Koktejly
        {
            get { return koktejly; }
            set { koktejly = value; }
        }

        private Koktejl selectedKoktejl;
        public Koktejl SelectedKoktejl
        {
            get { return selectedKoktejl; }
            set { SetIfChanged(ref selectedKoktejl, value); }
        }

        private int cenaKoktejlu;
        [XmlIgnore]
        public int CenaKoktejlu
        {
            get { return cenaKoktejlu; }
            set { SetIfChanged(ref cenaKoktejlu, value); }
        }

        private ObservableCollection<Stul> stoly;
        public ObservableCollection<Stul> Stoly
        {
            get { return stoly; }
            set { stoly = value; }
        }

        
        private Stul selectedStul;
        public Stul SelectedStul
        {
            get { return selectedStul; }
            set { SetIfChanged(ref selectedStul, value); }
        }

        private Objednavka selectedObjednavka;
        public Objednavka SelectedObjednavka
        {
            get { return selectedObjednavka; }
            set { SetIfChanged(ref selectedObjednavka, value); }
        }

        private ObservableCollection<Objednavka> zaplaceno;
        public ObservableCollection<Objednavka> Zaplaceno
        {
            get { return zaplaceno; }
            set { zaplaceno = value; }
        }

        private int cena;
        public int Cena
        {
            get { return cena; }
            set { SetIfChanged(ref cena, value); }
        }

        [XmlIgnore]
        private string path;
        public string Path
        {
            get { return path; }
            set { SetIfChanged(ref path, value); }
        }

        [XmlIgnore]
        public RelayCommand Pridat { get; private set; }
        [XmlIgnore]
        public RelayCommand Zaplatit { get; private set; }
        [XmlIgnore]
        public RelayCommand ZaplatitVse { get; private set; }
        [XmlIgnore]
        public RelayCommand Ulozit { get; private set; }
        [XmlIgnore]
        public RelayCommand Nacist { get; private set; }
        [XmlIgnore]
        public RelayCommand Novy { get; private set; }
        [XmlIgnore]
        public RelayCommand Tisk { get; private set; }
        [XmlIgnore]
        public RelayCommand HTML { get; private set; }

        public Bar()
        {
            Path = "bar.xml";
            
            commands();

            Koktejly = new ObservableCollection<Koktejl>();
            Stoly = new ObservableCollection<Stul>();
            Zaplaceno = new ObservableCollection<Objednavka>();
        }

        private void init()
        {
            Koktejly.Add(new Koktejl() { Nazev = "CubaLibre", Cena = 50, Ingredience = new List<string>() { "cola", "rum", "citron" } });
            Koktejly.Add(new Koktejl() { Nazev = "Mochito", Cena = 50, Ingredience = new List<string>() { "cola", "rum", "mata" } });

            Stoly.Add(new Stul() { ID = 1});
            Stoly.Add(new Stul() { ID = 2});
            Stoly[0].Objednavky.Add(new Objednavka("Zabi Hlen", 52));
        }

        private void commands()
        {
            Pridat = new RelayCommand(
                            (o) =>
                            {
                                if (CenaKoktejlu > 0)
                                {
                                    SelectedStul.Objednavky.Add(new Objednavka(SelectedKoktejl.Nazev, CenaKoktejlu));
                                    CenaKoktejlu = 0;
                                }
                                else
                                {
                                    SelectedStul.Objednavky.Add(new Objednavka(SelectedKoktejl.Nazev, SelectedKoktejl.Cena));
                                }

                                SelectedStul.Cena = 0;
                                foreach (var s in SelectedStul.Objednavky) SelectedStul.Cena += s.Cena;
                            });

            Zaplatit = new RelayCommand(
                            (o) =>
                            {
                                Zaplaceno.Add(SelectedObjednavka);
                                SelectedStul.Objednavky.Remove(SelectedObjednavka);

                                Cena = 0;
                                foreach (var c in Zaplaceno) Cena += c.Cena;
                            });

            Ulozit = new RelayCommand(
                            (o) =>
                            {
                                Serializace.ToXML(this, Path);
                            });

            Nacist = new RelayCommand(
                            (o) =>
                            {
                                Bar bar = Serializace.FromXML(Path);

                                this.Koktejly.Clear();
                                SelectedKoktejl = null;
                                this.Stoly.Clear();
                                SelectedStul = null;
                                SelectedObjednavka = null;
                                this.Zaplaceno.Clear();

                                foreach (var x in bar.Koktejly) this.Koktejly.Add(x);
                                foreach (var x in bar.Stoly) this.Stoly.Add(x);
                                foreach (var x in bar.Zaplaceno) this.Zaplaceno.Add(x);

                                //this.Koktejly = bar.Koktejly;
                                //this.Stoly = bar.Stoly;
                                //this.Zaplaceno = bar.Zaplaceno;

                                count();
                            });
            
            Novy = new RelayCommand(
                            (o) =>
                            {
                                init();
                                count();
                            });

            Tisk = new RelayCommand(
                            (o) =>
                            {
                                PrintDialog print = new PrintDialog();
                                print.ShowDialog();
                                //print.PrintVisual();
                            });

            HTML = new RelayCommand(
                            (o) =>
                            {
                                init();
                                count();
                            });
        }

        private void count()
        {
            foreach (var s in Stoly)
            {
                s.Cena = 0;
                foreach (var o in s.Objednavky) s.Cena += o.Cena;
            }

            Cena = 0;
            foreach (var c in Zaplaceno) Cena += c.Cena;
        }
    }
}
