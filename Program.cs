using System;
using System.Collections.Generic;

namespace ConsoleApp3
{
    internal class Program
    {
        // ====== Donnees du compte ======
        static string codePin = "1234";
        static long solde = 500000;
        const int MaxTentatives = 3;

        class Operation
        {
            public DateTime Date;
            public string Type;
            public long Montant;
        }

        static readonly List<Operation> historique = new List<Operation>();

        // ====== Point d'entree ======
        static void Main(string[] args)
        {
            Console.Title = "UAZ BANKING - Distributeur Automatique";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (!Authentification())
            {
                Console.WriteLine();
                MessageErreur("Carte bloquee. Trop de tentatives incorrectes.");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour fermer...");
                return;
            }

            MenuPrincipal();

            Console.WriteLine();
            MessageSucces("Merci d'avoir utilise notre distributeur !");
            Console.WriteLine();
            Pause("Appuyez sur une touche pour fermer...");
        }

        // ====== Authentification ======
        static bool Authentification()
        {
            Console.Clear();
            AfficherEntete();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("          DISTRIBUTEUR AUTOMATIQUE");
            Console.WriteLine("                BIENVENUE !");
            Console.ResetColor();
            Console.WriteLine();
            Ligne();
            Console.WriteLine();

            int tentatives = MaxTentatives;

            while (tentatives > 0)
            {
                Console.Write("Veuillez saisir votre code PIN : ");
                string saisie = Console.ReadLine();

                if (saisie == codePin)
                {
                    Console.WriteLine();
                    MessageSucces("Authentification reussie ! Bienvenue.");
                    Console.WriteLine();
                    Pause("Appuyez sur une touche pour continuer...");
                    return true;
                }

                tentatives--;
                Console.WriteLine();
                MessageErreur("Code PIN incorrect !");
                if (tentatives > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Tentatives restantes : " + tentatives);
                    Console.ResetColor();
                }
            }

            return false;
        }

        // ====== Menu principal ======
        static void MenuPrincipal()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherEntete();
                AfficherTitre("MENU PRINCIPAL");
                Ligne();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solde disponible : " + solde + " Ar");
                Console.ResetColor();
                Console.WriteLine();
                Ligne();
                Console.WriteLine();
                Console.WriteLine("[1]  Consulter mon solde");
                Console.WriteLine("[2]  Effectuer un retrait");
                Console.WriteLine("[3]  Effectuer un depot");
                Console.WriteLine("[4]  Changer mon code PIN");
                Console.WriteLine("[5]  Historique des operations");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[0]  Quitter");
                Console.ResetColor();
                Console.WriteLine();
                Ligne();
                Console.WriteLine();
                Console.Write("Votre choix : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ConsulterSolde();
                        break;
                    case "2":
                        Retrait();
                        break;
                    case "3":
                        Depot();
                        break;
                    case "4":
                        ChangerPin();
                        break;
                    case "5":
                        AfficherHistorique();
                        break;
                    case "0":
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine();
                        MessageErreur("Choix invalide !");
                        Console.WriteLine();
                        Pause("Appuyez sur une touche pour continuer...");
                        break;
                }
            }
        }

        // ====== [1] Consulter le solde ======
        static void ConsulterSolde()
        {
            Console.Clear();
            AfficherEntete();
            AfficherTitre("CONSULTATION DU SOLDE");
            Ligne();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Solde disponible : " + solde + " Ar");
            Console.ResetColor();
            Console.WriteLine();
            Pause("Appuyez sur une touche pour continuer...");
        }

        // ====== [2] Retrait ======
        static void Retrait()
        {
            Console.Clear();
            AfficherEntete();
            AfficherTitre("RETRAIT D'ARGENT");
            Ligne();
            Console.WriteLine();
            Console.WriteLine("Solde disponible : " + solde + " Ar");
            Console.WriteLine();
            Console.Write("Montant a retirer : ");

            if (!long.TryParse(Console.ReadLine(), out long montant) || montant <= 0)
            {
                Console.WriteLine();
                MessageErreur("Montant invalide !");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour continuer...");
                return;
            }

            if (montant > solde)
            {
                Console.WriteLine();
                MessageErreur("Solde insuffisant !");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour continuer...");
                return;
            }

            solde -= montant;
            Enregistrer("Retrait", montant);

            Console.WriteLine();
            MessageSucces("Retrait effectue avec succes.");
            Console.WriteLine("Nouveau solde : " + solde + " Ar");
            Console.WriteLine();
            Pause("Appuyez sur une touche pour continuer...");
        }

        // ====== [3] Depot ======
        static void Depot()
        {
            Console.Clear();
            AfficherEntete();
            AfficherTitre("DEPOT D'ARGENT");
            Ligne();
            Console.WriteLine();
            Console.Write("Montant a deposer : ");

            if (!long.TryParse(Console.ReadLine(), out long montant) || montant <= 0)
            {
                Console.WriteLine();
                MessageErreur("Montant invalide !");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour continuer...");
                return;
            }

            solde += montant;
            Enregistrer("Depot", montant);

            Console.WriteLine();
            MessageSucces("Depot effectue avec succes.");
            Console.WriteLine("Nouveau solde : " + solde + " Ar");
            Console.WriteLine();
            Pause("Appuyez sur une touche pour continuer...");
        }

        // ====== [4] Changer le code PIN ======
        static void ChangerPin()
        {
            Console.Clear();
            AfficherEntete();
            AfficherTitre("CHANGEMENT DU CODE PIN");
            Ligne();
            Console.WriteLine();
            Console.Write("Ancien code PIN : ");
            string ancien = Console.ReadLine();

            if (ancien != codePin)
            {
                Console.WriteLine();
                MessageErreur("Ancien code PIN incorrect !");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour continuer...");
                return;
            }

            Console.Write("Nouveau code PIN (4 chiffres) : ");
            string nouveau = Console.ReadLine();

            if (!PinValide(nouveau))
            {
                Console.WriteLine();
                MessageErreur("Le code PIN doit contenir exactement 4 chiffres !");
                Console.WriteLine();
                Pause("Appuyez sur une touche pour continuer...");
                return;
            }

            codePin = nouveau;
            Console.WriteLine();
            MessageSucces("Code PIN modifie avec succes.");
            Console.WriteLine();
            Pause("Appuyez sur une touche pour continuer...");
        }

        static bool PinValide(string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4) return false;
            foreach (char c in pin)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        // ====== [5] Historique ======
        static void AfficherHistorique()
        {
            Console.Clear();
            AfficherEntete();
            AfficherTitre("HISTORIQUE DES OPERATIONS");
            Ligne();
            Console.WriteLine();

            if (historique.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Aucune operation effectuee.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Les cinq dernieres operations :");
                Console.ResetColor();
                Console.WriteLine();

                // Les plus recentes en premier, 5 maximum
                int numero = 1;
                for (int i = historique.Count - 1; i >= 0 && numero <= 5; i--, numero++)
                {
                    Operation op = historique[i];
                    Console.WriteLine(numero + ". " + op.Date.ToString("dd/MM/yyyy HH:mm")
                                      + " | " + op.Type + " | " + op.Montant + " Ar");
                }
            }

            Console.WriteLine();
            Pause("Appuyez sur une touche pour continuer...");
        }

        static void Enregistrer(string type, long montant)
        {
            historique.Add(new Operation
            {
                Date = DateTime.Now,
                Type = type,
                Montant = montant
            });
        }

        // ====== Affichage ======
        static void AfficherEntete()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("                   UAZ BANKING");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void AfficherTitre(string titre)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(titre);
            Console.ResetColor();
            Console.WriteLine();
        }

        static void Ligne()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();
        }

        static void MessageSucces(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> " + message);
            Console.ResetColor();
        }

        static void MessageErreur(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> " + message);
            Console.ResetColor();
        }

        static void Pause(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(message);
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}