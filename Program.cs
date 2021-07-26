using System;
using System.Collections.Generic;
using System.IO;

namespace pendu
{
    class Program
    {
        //Parcours le mot, affiche un underscore si la lettre ne correspond pas, affiche la lettre si elle correspond
        static void AfficherMot(string mot, List<char> lettres) 
        {
            string motCache = "";
            for (int i = 0; i< mot.Length; i++)
            {
                if (lettres.Contains(mot[i]))
                {
                    motCache += mot[i] + " ";
                }
                else
                {
                    motCache += "_ ";

                }
            }
            Console.Write(motCache);
        }

        //Vérifie quand on a deviné toutes les lettres en vidant un string témoin
        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            foreach (var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot == "")
            {
                return true;
            }
            else
            {
                return false;
            }
            
            
        }

        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {
            
            
            while (true)
            {
                Console.WriteLine("Entrez une lettre");
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                else
                {
                    Console.WriteLine("ERREUR, vous devez rentrer une seul lettre");
                }
                

            }
            
        }

        static void DevinerMot(string mot)
        {
            var lettresDevinees = new List<char>();
            var lettresPasDansLeMot = new List<char>();
            const int NB_VIES = 6;
            int nbViesRestantes = NB_VIES;
            while (nbViesRestantes > 0) 
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - nbViesRestantes]);

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    lettresDevinees.Add(lettre);
                    Console.WriteLine("Cette lettre est dans le mot");
                    if (ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        Console.WriteLine("Gagné !");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Cette lettre n'est pas dans le mot");
                    if (!lettresPasDansLeMot.Contains(lettre))
                    {
                        lettresPasDansLeMot.Add(lettre);
                        nbViesRestantes--;
                    }                    
                    Console.WriteLine($"Il vous reste {nbViesRestantes} vies");
                }
                if (lettresPasDansLeMot.Count > 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(", ", lettresPasDansLeMot));
                }

                Console.WriteLine();
            }
            if (nbViesRestantes == 0)
            {
                Console.WriteLine($"PERDU ! Le mot était {mot}");
                Console.WriteLine(Ascii.PENDU[NB_VIES - nbViesRestantes]);
            }
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erreur de lecture du fichier {nomFichier} ({ex.Message}");
            }
            return null;
        }

        static bool DemanderRejouer()
        {
            var rejouer = DemanderUneLettre("Voulez-vous rejouer ? (Entrez 'o' pour rejouer) : ");
            if (rejouer == 'O')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if (mots == null || (mots.Length == 0))
            {
                Console.WriteLine("La liste de mot est vide");
            }
            else
            {
                while (true)
                {
                    Random rand = new Random();
                    string mot = mots[rand.Next(0, mots.Length)].Trim().ToUpper();
                    DevinerMot(mot);
                    if (!DemanderRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                
            }
            Console.WriteLine("Au revoir");
            
        }
    }
}
