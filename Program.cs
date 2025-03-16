using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;


namespace ProiectNr3{

    abstract class ElementConcret{
        public abstract double SuprafataVopsitaMP();
        //toate elem concrete au aceasta metoda;

         public abstract string ToCsv(string parentName = ""); //pt CSV export
    }

    class ProfilMetalic: ElementConcret{
        private string Format;
        private double Latime;
        private double Lungime;
        private string Nume;
        private double SuprafataVopsita;

        public ProfilMetalic(string format,string nume){
            Format=format;
            Nume=nume;
            //extarg lungimea si latimea din format
            string width="";
            string length="";
            bool ok=false;//cand gasesc primul nr il fac true ca sa pot salva al doilea nr

            foreach(char c in Format){
                if(char.IsDigit(c)){
                    if(ok==false){
                        width+=c;
                    }
                    else{
                        length+=c;
                    }
                }
                else if(width!=""){
                    ok=true;
                }
                
            }
            Latime=double.Parse(width, CultureInfo.InvariantCulture);
            Lungime=double.Parse(length, CultureInfo.InvariantCulture);
            SuprafataVopsita=6*Latime*Lungime;
        }

        public override double SuprafataVopsitaMP(){
            return SuprafataVopsita;
        }
        public override string ToCsv(string parentName = "")
        {
            return $"ProfilMetalic,{Nume},{parentName},{SuprafataVopsita}mp \n";
            //return $"ProfilMetalic,{Nume},{Latime},{Lungime},{SuprafataVopsita} \n";
        }
    }

class FlansaDeTabla: ElementConcret{
    private double Lungime;
    private double Latime;
    private double Grosime;
    private double SuprafataDecupata;
    private int NrSuprafeteVopsite;
    private double SuprafataVopsita;
    private string Nume;

    public FlansaDeTabla(double lungime,double latime,double grosime, double suprDecupata,int nrFete,string nume){
        Lungime=lungime;
        Latime=latime;
        Grosime=grosime;
        SuprafataDecupata=suprDecupata;
        Nume=nume;
        if(nrFete>=1 && nrFete<=2){
            NrSuprafeteVopsite=nrFete;
            }
        else{
            throw new ArgumentException("Numar de argumente invalid.");
            }
        SuprafataVopsita=Latime*Lungime*NrSuprafeteVopsite-suprDecupata;

    }

    public override double SuprafataVopsitaMP(){
        return SuprafataVopsita;
    }
    public override string ToCsv(string parentName = "")
        {
            return $"FlansaDeTabla,{Nume},{parentName},{SuprafataVopsita}mp \n";
            //return $"FlansaDeTabla,{Lungime},{Latime},{Grosime},{SuprafataDecupata},{NrSuprafeteVopsite},{SuprafataVopsita} \n";
        }
}

class ElementSingular: ElementConcret{
    private double SuprafataVopsita;
    private string Nume;

    public ElementSingular(double suprafata,string nume){
        SuprafataVopsita=suprafata;
        Nume=nume;
    }

    public override double SuprafataVopsitaMP(){
        return SuprafataVopsita;
    }
    public override string ToCsv(string parentName = "")
        {
            return $"ElementSingular,{Nume},{parentName},{SuprafataVopsita}mp \n";
        }
}

abstract class Entitate{
    public abstract double SuprafataVopsitaNivel();
    public abstract string ToCsv(string parentName = ""); // pt CSV export

}
class PozitieSecundara:Entitate{
    private List<ElementConcret> ElemNiv5=new List<ElementConcret>();
    public string Nume{get;set;}

    public PozitieSecundara(string nume){
        Nume=nume;
    }
    public void AddElementConcret(ElementConcret elem){
         if (elem is ProfilMetalic || elem is FlansaDeTabla || elem is ElementSingular)
            {
                ElemNiv5.Add(elem);
            }
    }

    public override double SuprafataVopsitaNivel(){
        double sum=0;
        foreach(var elem in ElemNiv5){
            sum+=elem.SuprafataVopsitaMP();
        }
        return sum;
    }
    public override string ToCsv(string parentName = "")
        {
            string csvData ="";
            double rez=SuprafataVopsitaNivel();
            csvData+=$"PozitieSecundara,{Nume},{parentName},{rez}mp \n";
            foreach (var elem in ElemNiv5)
            {
                csvData +=  elem.ToCsv(this.Nume); 
            }
            return csvData;
        }
}

class PozitiePrincipala : Entitate{
    private List<PozitieSecundara> elemNiv4=new List<PozitieSecundara>();
    public string Nume{get;set;}

    public PozitiePrincipala(string nume){
        Nume=nume;
    }

    public void AddPozitieSecundara(PozitieSecundara ps){
        if(ps is PozitieSecundara){
            elemNiv4.Add(ps);
        }
    }

    public override double SuprafataVopsitaNivel(){
        double sum=0;
        foreach(var elem in elemNiv4){
            sum+=elem.SuprafataVopsitaNivel();
        }
        return sum;
    }
    public override string ToCsv(string parentName = "")
        {
            string csvData ="";
            double rez=SuprafataVopsitaNivel();
            csvData+=$"PozitiePrincipala,{Nume},{parentName},{rez}mp \n";
            foreach (var elem in elemNiv4)
            {
                csvData += elem.ToCsv(this.Nume); 
            }
            return csvData;
        }

}

class Plansa : Entitate{
    private List<PozitiePrincipala> elemNiv3=new List<PozitiePrincipala>();
    public string Nume{get;set;}

    public Plansa(string nume){
        Nume=nume;
    }

    public void AddPozitiePrincipala(PozitiePrincipala pp){
        if(pp is PozitiePrincipala){
            elemNiv3.Add(pp);
        }
    }

    public override double SuprafataVopsitaNivel(){
        double sum=0;
        foreach(var elem in elemNiv3){
            sum+=elem.SuprafataVopsitaNivel();
        }
        return sum;
    }
    public override string ToCsv(string parentName = "")
        {
            string csvData ="";
            double rez=SuprafataVopsitaNivel();
            csvData+=$"Plansa,{Nume},{parentName},{rez}mp \n";
            foreach (var elem in elemNiv3)
            {
                csvData += elem.ToCsv(this.Nume); 
            }
            return csvData;
        }
}

class Obiect : Entitate{
    private List<Plansa> elemNiv2=new List<Plansa>();
    public string Nume{get;set;}

    public Obiect(string nume){
        Nume=nume;
    }

    public void AddPlansa(Plansa p){
        if(p is Plansa){
            elemNiv2.Add(p);
        }
    }

    public override double SuprafataVopsitaNivel(){
        double sum=0;
        foreach(var elem in elemNiv2){
            sum+=elem.SuprafataVopsitaNivel();
        }
        return sum;
    }
    public override string ToCsv(string parentName = "")
        {
            string csvData ="";
            double rez=SuprafataVopsitaNivel();
            csvData+=$"Obiect,{Nume},{parentName},{rez}mp \n";
            foreach (var elem in elemNiv2)
            {
                csvData += elem.ToCsv(this.Nume); 
            }
            return csvData;
        }
}

class Proiect : Entitate{
    private List<Obiect> elemNiv1=new List<Obiect>();
    public string Nume{get;set;}

    public Proiect(string nume){
        Nume=nume;
    }

    public void AddObiect(Obiect o){
        if(o is Obiect){
            elemNiv1.Add(o);
        }
    }

    public override double SuprafataVopsitaNivel(){
        double sum=0;
        foreach(var elem in elemNiv1){
            sum+=elem.SuprafataVopsitaNivel();
        }
        return sum;
    }
    public override string ToCsv(string parentName = "")
    {
        string csvData ="";
        double rez=SuprafataVopsitaNivel();
        csvData+=$"Proiect,{Nume},{parentName},{rez}mp \n";

        foreach (var obiect in elemNiv1)
        {
            csvData += obiect.ToCsv(this.Nume); 
            
        }
        
        return csvData;
    }
}
    class Prog{
        public static void Main(string []args){
            ProfilMetalic pm1=new ProfilMetalic("HEA10x100","PFnume1");
            ProfilMetalic pm2=new ProfilMetalic("HEA20x100","PFnume2");
            ProfilMetalic pm3=new ProfilMetalic("HEA10x300","PFnume3");
            ProfilMetalic pm4=new ProfilMetalic("HEA14x140","PFnume4");

           
            Console.WriteLine(pm1.SuprafataVopsitaMP());
            FlansaDeTabla? f1=null,f2=null;
            try
            {
                 f1 = new FlansaDeTabla(100, 50, 5, 10, 3,"flansa1"); // Va arunca eroare
                Console.WriteLine($"Suprafata vopsita: {f1.SuprafataVopsitaMP()} mp");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Eroare: {ex.Message}");
            }
            try
            {
                 f2 = new FlansaDeTabla(100, 50, 5, 10, 2,"flansa2"); // Va arunca eroare
                Console.WriteLine($"Suprafata vopsita: {f2.SuprafataVopsitaMP()} mp");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Eroare: {ex.Message}");
            }

        //elem. singulare
        ElementSingular es1=new ElementSingular(0.2,"e1");
        ElementSingular es2=new ElementSingular(1.2,"e2");
        ElementSingular es3=new ElementSingular(0.5,"e3");
        ElementSingular es4=new ElementSingular(2.2,"e4");
        ElementSingular es5=new ElementSingular(0.6,"e5");
        Console.WriteLine(es1.SuprafataVopsitaMP());

        //pozitii secundare
        PozitieSecundara ps1=new PozitieSecundara("pozSec.nr1");
        ps1.AddElementConcret(pm1);
        ps1.AddElementConcret(es1);
        if (f2 != null) // Verific daca f2 este initializat inainte de a-l adauga
                {
                    ps1.AddElementConcret(f2);
                }
        Console.WriteLine("Suprafata de la prima pozitie secundra: "+ ps1.SuprafataVopsitaNivel());
        PozitieSecundara ps2=new PozitieSecundara("pozSec2");
        PozitieSecundara ps3=new PozitieSecundara("pozSec3");
        PozitieSecundara ps4=new PozitieSecundara("pozSec4");

        ps2.AddElementConcret(es2);
        ps2.AddElementConcret(es3);
        ps2.AddElementConcret(pm2);
        ps2.AddElementConcret(pm3);

        ps3.AddElementConcret(es2);
        ps3.AddElementConcret(es3);
        ps3.AddElementConcret(pm4);
        ps3.AddElementConcret(pm3);

        ps4.AddElementConcret(es4);
        ps4.AddElementConcret(es3);
        ps4.AddElementConcret(pm4);
        ps4.AddElementConcret(pm3);
        
        // poz. principale
        PozitiePrincipala pp1 = new PozitiePrincipala("PozitiePrincipala1");
        pp1.AddPozitieSecundara(ps1);
        pp1.AddPozitieSecundara(ps2);
        Console.WriteLine($"Suprafata vopsita la {pp1.Nume}: {pp1.SuprafataVopsitaNivel()} mp");
        PozitiePrincipala pp2 = new PozitiePrincipala("PozitiePrincipala2");
        pp2.AddPozitieSecundara(ps3);
        PozitiePrincipala pp3 = new PozitiePrincipala("PozitiePrincipala3");
        pp3.AddPozitieSecundara(ps1);
        pp3.AddPozitieSecundara(ps4);
        PozitiePrincipala pp4 = new PozitiePrincipala("PozitiePrincipala4");
        pp4.AddPozitieSecundara(ps2);
        pp4.AddPozitieSecundara(ps3);
        pp4.AddPozitieSecundara(ps4);

        //Planse
        Plansa plansa1 = new Plansa("Plansa1");
        plansa1.AddPozitiePrincipala(pp1);
        plansa1.AddPozitiePrincipala(pp2);
        plansa1.AddPozitiePrincipala(pp4);
        Console.WriteLine($"Suprafata vopsita la {plansa1.Nume}: {plansa1.SuprafataVopsitaNivel()} mp");
        Plansa plansa2 = new Plansa("Plansa2");
        plansa2.AddPozitiePrincipala(pp4);
        plansa2.AddPozitiePrincipala(pp3);
    

        //Obiecte
        Obiect obiect1 = new Obiect("Obiect1");
        obiect1.AddPlansa(plansa1);
        Console.WriteLine($"Suprafata vopsita la {obiect1.Nume}: {obiect1.SuprafataVopsitaNivel()} mp");
        Obiect obiect2 = new Obiect("Obiect2");
        obiect2.AddPlansa(plansa1);
        obiect2.AddPlansa(plansa2);


        //Proiect
        Proiect proiect1 = new Proiect("Proiect1");
        proiect1.AddObiect(obiect1);
        proiect1.AddObiect(obiect2);

        Console.WriteLine($"Suprafata vopsita in {proiect1.Nume}: {proiect1.SuprafataVopsitaNivel()} mp");

        // export proiect in CSV
        string filePath = "proiect.csv"; // calea fisierului
        // adaug header-ul in fisier
        File.WriteAllText(filePath, "TipElement,Nume,IdParinte,SuprafataVopsita(mp)\n");
        // adaug datele proiectului
        File.AppendAllText(filePath, proiect1.ToCsv());
        Console.WriteLine($"Proiectul a fost exportat in {filePath}");
        string fp="aa.csv";
        File.WriteAllText(fp,obiect1.ToCsv());
        }
    }
}

