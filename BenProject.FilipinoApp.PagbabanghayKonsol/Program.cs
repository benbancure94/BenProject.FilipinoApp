using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BenProject.FilipinoApp.PagbabanghayKonsol
{
    public class Banghay
    {
        Regex rSimulaSaPatinig = new Regex("^[aeiou]", RegexOptions.IgnoreCase);
        Regex rSimulaSaL = new Regex("^l", RegexOptions.IgnoreCase);
        Regex rTaposSaD = new Regex("d$", RegexOptions.IgnoreCase);

        string[] mgaUnlapi = new string[] { "mag", "nag", "pag", "mang", "nang", "pang", "ma", "na", "ka", "pa", "i" };
        string[] mgaGitlapi = new string[] { "um", "in" };
        string[] mgaHulapi = new string[] { "an", "in" };

        public string Salita = string.Empty;

        public List<BanghayKaanyuan> MgaMayLapi = new List<BanghayKaanyuan>();

        public Banghay(string salita)
        {
            bool simulaSaPatinig = rSimulaSaPatinig.IsMatch(salita);
            bool simulaSaL = rSimulaSaL.IsMatch(salita);
            bool taposSaD = rTaposSaD.IsMatch(salita);
            bool simulaSaDARA = Regex.IsMatch(salita, "^d[aeiou]r[aeiou]");
            bool taposSaPatinig = Regex.IsMatch(salita, "[aeiou]$");
            string simulangTitik = Regex.Match(salita, "^[abkdghlmnprstwy]", RegexOptions.IgnoreCase).Value;
            string simulangKP = Regex.Match(salita, "^(ng|[bkdghlmnprstwy])?[aeiou]", RegexOptions.IgnoreCase).Value;
            string taposNaKP = Regex.Match(salita, "[aeiou](ng|[bkdghlmnprstwy])?$", RegexOptions.IgnoreCase).Value;
            bool mayHulingO = Regex.IsMatch(taposNaKP, "o");

            Salita = salita;

            var bk = new BanghayKaanyuan()
            {
                Panlapi = "- (KP)",
                Uri = PanlapiUri.Wala,
                MgaAnyo = new List<string>() { simulangKP + salita }
            };

            if (simulangTitik == "d" && !simulaSaDARA)
            {
                bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
            }

            MgaMayLapi.Add(bk);

            foreach (var unlapi in mgaUnlapi)
            {
                bool panlapingNagtataposSaG = Regex.IsMatch(unlapi, "g$", RegexOptions.IgnoreCase);
                bool panlapingNagtataposSaNG = Regex.IsMatch(unlapi, "ng$", RegexOptions.IgnoreCase);
                bool panlapingNagtataposSaPatinig = Regex.IsMatch(unlapi, "[aeiou]$", RegexOptions.IgnoreCase);

                bk = new BanghayKaanyuan()
                {
                    Panlapi = unlapi,
                    Uri = PanlapiUri.Unlapi,
                    MgaAnyo = new List<string>() { unlapi + (simulaSaPatinig && !panlapingNagtataposSaPatinig ? "-" : string.Empty) + salita }
                };

                if (panlapingNagtataposSaPatinig && simulangTitik == "d" && !simulaSaDARA)
                {
                    bk.MgaAnyo.Add(unlapi + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
                }

                if (panlapingNagtataposSaNG)
                {
                    if (simulaSaPatinig || simulangTitik == "k")
                    {
                        bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^k?", "ng", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                    }

                    else if (simulangTitik == "b" || simulangTitik == "p")
                    {
                        bk.MgaAnyo.Add(Regex.Replace(unlapi, "ng$", "m", RegexOptions.IgnoreCase) + salita);
                        bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^[bp]?", "m", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                    }

                    else if (simulangTitik == "d" || simulangTitik == "t" || simulangTitik == "s" || simulangTitik == "l")
                    {
                        bk.MgaAnyo.Add(Regex.Replace(unlapi, "ng$", "n", RegexOptions.IgnoreCase) + salita);
                        if (simulangTitik != "l")
                        {
                            bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^[dst]?", "n", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                        }
                    }
                }

                MgaMayLapi.Add(bk);

                bk = new BanghayKaanyuan()
                {
                    Panlapi = unlapi + " (KP)",
                    Uri = PanlapiUri.Unlapi,
                    MgaAnyo = new List<string>() { unlapi + (simulaSaPatinig && !panlapingNagtataposSaPatinig ? "-" : string.Empty) + simulangKP + salita }
                };

                if (simulangTitik == "d" && !simulaSaDARA)
                {
                    bk.MgaAnyo.Add(unlapi + simulangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
                    if (panlapingNagtataposSaPatinig)
                    {
                        bk.MgaAnyo.Add(unlapi + Regex.Replace(simulangKP, "^d", "r", RegexOptions.IgnoreCase) + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
                    }
                }

                if (panlapingNagtataposSaNG)
                {
                    // 
                    // kapag simula sa patinig
                    // - mang-abot, mang-inis, mang-uyam *
                    // - mangaway/mang-away, manginom/mang-inom, mangulila/mang-ulila *
                    // kapag simula sa patinig (KP)
                    // - mang-aabot, mang-iinis, mang-uuyam *
                    // - mangangaway/mang-aaway, manginginom/mang-iinom, mangungulila/mang-uulila *
                    // kapag simula sa b, p 
                    // - mangbunga/mambunga/mamunga, mangbasa/mambasa/mamasa *  
                    // kapag simula sa b, p (KP)
                    // - mangbubunga/mambubunga/mamumunga, mangbabasa/mambabasa/mamamasa *
                    // kapag simula sa d, s, t
                    // - mangdungaw/mandungaw/manungaw, mangsisi/mansisi/manisi, mangtaboy/mantaboy/manaboy
                    // kapag simula sa d, s, t (KP)
                    // - mangdudungaw/mandudungaw/manunungaw/mangdurungaw/mandurungaw, mangsisisi/mansisisi/maninisi, mangtataboy/mantataboy/mananaboy
                    // kapag simula sa l
                    // - mangligaw/manligaw
                    // kapag simula sa l (KP)
                    // - mangliligaw/manliligaw
                    // kapag simula sa k
                    // - mangkatay/mangatay *
                    // kapag simula sa k (KP)
                    // - mangkakatay/mangangatay *
                    // 

                    if (simulaSaPatinig || simulangTitik == "k")
                    {
                        bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) + 
                            Regex.Replace(simulangKP, "^k?", "ng", RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^k?", "ng", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                    }

                    else if (simulangTitik == "b" || simulangTitik == "p")
                    {
                        bk.MgaAnyo.Add(Regex.Replace(unlapi, "ng$", "m", RegexOptions.IgnoreCase) + simulangKP + salita);
                        bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) +
                            Regex.Replace(simulangKP, "^[bp]?", "m", RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^[bp]?", "m", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                    }

                    else if (simulangTitik == "d" || simulangTitik == "t" || simulangTitik == "s" || simulangTitik == "l")
                    {
                        bk.MgaAnyo.Add(Regex.Replace(unlapi, "ng$", "n", RegexOptions.IgnoreCase) + simulangKP + salita);

                        if (simulangTitik == "d" && !simulaSaDARA)
                        {
                            bk.MgaAnyo.Add(Regex.Replace(unlapi, "ng$", "n", RegexOptions.IgnoreCase) + simulangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
                        }

                        if (simulangTitik != "l")
                        {
                            bk.MgaAnyo.Add(
                            Regex.Replace(unlapi, "ng$", string.Empty, RegexOptions.IgnoreCase) +
                            Regex.Replace(simulangKP, "^[dst]?", "n", RegexOptions.IgnoreCase) +
                            Regex.Replace(salita, "^" + simulangKP, Regex.Replace(simulangKP, "^[dst]?", "n", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase));
                        }
                    }
                }

                MgaMayLapi.Add(bk);
            }

            foreach (var gitlapi in mgaGitlapi)
            {
                bk = new BanghayKaanyuan()
                {
                    Panlapi = gitlapi,
                    Uri = PanlapiUri.Gitlapi,
                    MgaAnyo = new List<string>()
                    {
                        Regex.Replace(salita, "^" + simulangKP, simulangKP.Insert(simulaSaPatinig ? 0 : 1, gitlapi), RegexOptions.IgnoreCase)
                    }
                };
                MgaMayLapi.Add(bk);

                if (gitlapi == "in" && (simulangTitik == "l" || simulangTitik == "y"))
                {
                    bk.MgaAnyo.Add("ni" + salita);
                }

                bk = new BanghayKaanyuan()
                {
                    Panlapi = gitlapi + " (KP)",
                    Uri = PanlapiUri.Gitlapi,
                    MgaAnyo = new List<string>()
                    {
                        simulangKP.Insert(simulaSaPatinig ? 0 : 1, gitlapi) + salita
                    }
                };

                if (simulangTitik == "d")
                {
                    bk.MgaAnyo.Add(simulangKP.Insert(simulaSaPatinig ? 0 : 1, gitlapi) + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase));
                }

                if (gitlapi == "in" && (simulangTitik == "l" || simulangTitik == "y"))
                {
                    bk.MgaAnyo.Add("ni" + simulangKP + salita);
                }

                MgaMayLapi.Add(bk);
            }

            foreach (var hulapi in mgaHulapi)
            {
                bk = new BanghayKaanyuan()
                {
                    Panlapi = hulapi,
                    Uri = PanlapiUri.Hulapi,
                    MgaAnyo = new List<string>()
                    {
                        salita + hulapi
                    }
                };

                if (mayHulingO)
                {
                    bk.MgaAnyo.Add(Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + hulapi);
                }

                if (taposSaPatinig)
                {
                    bk.MgaAnyo.Add(salita + "h" + hulapi);
                    if (mayHulingO)
                    {
                        bk.MgaAnyo.Add(Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + "h" + hulapi);
                    }
                }

                if (taposSaD)
                {
                    bk.MgaAnyo.Add(Regex.Replace(salita, "d$", "r") + hulapi);
                    if (mayHulingO)
                    {
                        bk.MgaAnyo.Add(Regex.Replace(Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase), "d$", "r") + hulapi);
                    }
                }

                // PANANDA
                // K1, K2, ... -> mga katinig kasama (ng)
                // P1, P2, ... -> mga patinig
                // | -> hati sa pantig

                // kung magtatapos sa P1 | K1 P2 K2 -> P1 K1 | K2 kapag K1 <> 'h' at K1 <> K2
                // putol -> putlin, dukal -> duklan, bukas -> buksan, hukay -> hukyan, sakay -> sakyan, tulad -> tuldin, pukol -> puklin, asin -> asnan, ibis -> ibsan, sunod -> sundan,sundin, kamit -> kamtan,kamtin, likod -> likdan, tangan -> tangnan, tingin -> tingnan
                // pasubali: ginip -> ni-m -> gimpan, halik -> li-g -> hagkan, harap -> hindi kailanman harpin, dinig -> ni-ng -> dinggin, ganap -> gampan



                // kung magtatapos sa P1 | K1 P2 K2 -> P1 K2 | K1
                // talab -> tablan, dilim -> dimlan, 
                // pasubali: harap -> hindi kailanman haprin

                // kung magtatapos sa P1 | P2 K1 -> P1 | K1 (P1 at P2 ay 'i', P1 ay 'o/u' at P2 ay 'o')
                // tuod -> turan, buod -> buran, siil -> silin, tiis -> tisin

                // kung magtatapos sa PI | K1 P2 -> P1 | K1 o P1 - K1
                // tubo -> tub-an, tuban, 

                MgaMayLapi.Add(bk);

                bk = new BanghayKaanyuan()
                {
                    Panlapi = hulapi + " - (KP)",
                    Uri = PanlapiUri.Hulapi,
                    MgaAnyo = new List<string>() { simulangKP + salita + hulapi }
                };

                if (mayHulingO)
                {
                    bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + hulapi);
                }

                if (taposSaPatinig)
                {
                    bk.MgaAnyo.Add(simulangKP + salita + "h" + hulapi);
                    if (mayHulingO)
                    {
                        bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + "h" + hulapi);
                    }
                }

                if (taposSaD)
                {
                    bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, "d$", "r") + hulapi);
                    if (mayHulingO)
                    {
                        bk.MgaAnyo.Add(simulangKP + Regex.Replace(Regex.Replace(salita, taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase), "d$", "r") + hulapi);
                    }
                }

                if (simulangTitik == "d" && !simulaSaDARA)
                {
                    bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) + hulapi);
                    if (mayHulingO)
                    {
                        bk.MgaAnyo.Add(simulangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + hulapi);
                    }
                    if (taposSaPatinig)
                    {
                        bk.MgaAnyo.Add(simulangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) + "h" + hulapi);
                        if (mayHulingO)
                        {
                            bk.MgaAnyo.Add(simulangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + "h" + hulapi);
                        }
                    }
                    if (taposSaD)
                    {
                        bk.MgaAnyo.Add(simulangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), "d$", "r") + hulapi);
                        if (mayHulingO)
                        {
                            bk.MgaAnyo.Add(simulangKP + Regex.Replace(Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), taposNaKP, Regex.Replace(taposNaKP, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase), "d$", "r") + hulapi);
                        }
                    }
                }
                
                MgaMayLapi.Add(bk);
            }
        }
    }

    public class BanghayKaanyuan
    {
        public string Panlapi { get; set; }
        public PanlapiUri Uri { get; set; }
        public List<string> MgaAnyo { get; set; }
    }

    public enum PanlapiUri
    {
        Wala, Unlapi, Gitlapi, Hulapi
    }

    public enum SalitaDiin
    {
        Malumay, Malumi, Mabilis, Maragsa
    }

    public enum Unlapi
    {
        Mag, Nag, Pag
    }

    class Program
    {
        static string[] mgaUnlapi = new string[] { "mag", "nag", "pag", "mang", "nang", "pang", "ma", "na", "ka", "pa", "i" };

        static string LumikhaNgTsart(string[] ulunan, string[][][] hanay)
        {
            string tsart = string.Empty;
            string guhit = string.Empty;
            string _ulunan = string.Empty;
            string _mgaHanay = string.Empty;

            List<int> habaTudling = new List<int>();

            for (int i = 0; i <= ulunan.Length; i++)
            {
                if (i > 0) guhit += "-";
                guhit += "+";
                if (i < ulunan.Length)
                {
                    int ht = hanay.Max(x => x[i].Max(y => y.Length));
                    habaTudling.Add(ulunan[i].Length < ht ? ht : ulunan[i].Length);

                    guhit += "-";
                    for (int j = 0; j < habaTudling[i]; j++)
                    {
                        guhit += "-";
                    }
                }
            }

            for (int i = 0; i <= ulunan.Length; i++)
            {
                if (i > 0) _ulunan += " ";
                _ulunan += "|";
                if (i < ulunan.Length)
                {
                    _ulunan += " ";
                    _ulunan += ulunan[i];

                    for (int j = 0; j < habaTudling[i] - ulunan[i].Length; j++)
                    {
                        _ulunan += " ";
                    }
                }
            }

            for (int i = 0; i < hanay.Length; i++)
            {
                int m = hanay[i].Max(x => x.Length);
                for (int j = 0; j < m; j++)
                {
                    for (int k = 0; k <= ulunan.Length; k++)
                    {
                        if (k > 0) _mgaHanay += " ";
                        _mgaHanay += "|";
                        if (k < ulunan.Length)
                        {
                            _mgaHanay += " ";
                            if (hanay[i][k].Length > j)
                            {
                                _mgaHanay += hanay[i][k][j];
                            }

                            for (int l = 0; l < habaTudling[k] - (hanay[i][k].Length > j ? hanay[i][k][j].Length : 0); l++)
                            {
                                _mgaHanay += " ";
                            }
                        }
                    }
                    _mgaHanay += Environment.NewLine;
                }
            }

            tsart = 
                 guhit + Environment.NewLine +
                 _ulunan + Environment.NewLine +
                 guhit + Environment.NewLine +
                 _mgaHanay +
                 guhit;
            return tsart;
        }

        //string[] Lapiin(string salita)
        //{ 
        //    string[] banghay = new string[] { };
        //    bool simulaSaPatinig = rSimulaSaPatinig.IsMatch(salita);
        //    banghay.Concat(new string[] {  });
        //    return new string[] { };
        //}

        /*
            Suliranin:
        
            May mga salitang ugat na Tagalog bang nagtataglay ng:
            ia - (biak -> biyak, hindi lahian)
            io - ?
            ua - (buang -> buwang; luad -> luwad)
            ui - ? (hindi buuin)
            awo - (tamawo?; tao, kamao, bilao)
            iwo - ?
            uwo - ?
            ayi - (oyayi, hayin?, hindi buhayin)
            iyi - ?
            uyi - ? (hindi babuyin)
        */

        static string[] Lapiin(
            string salita,
            PanlapiUri panlapiUri = PanlapiUri.Wala,
            SalitaDiin salitaDiin = SalitaDiin.Malumay,
            string panlapi = "",
            bool ulitinAngUnangKP = true,
            bool gawingRAngUnangTitik = true,
            bool gawingRAngIkalawangUlit = true
        )
        {
            string[] banghay = new string[] { };
            string unangKP = Regex.Match(salita, "^(ng|[bkdghlmnprstwy])?[aeiou]", RegexOptions.IgnoreCase).Value;
            string hulingPK = Regex.Match(salita, "[aeiou](ng|[bkdghlmnprstwy])?$", RegexOptions.IgnoreCase).Value;
            string hulingPKPK = Regex.Match(salita, "[aeiou](ng|[bkdghlmnprstwy])[aeiou](ng|[bkdghlmnprstwy])$", RegexOptions.IgnoreCase).Value; // bULAK, sULAK, tIPON, iNOM, kULAM, tANIM, sILIM, bATID
            string hulingPKP = Regex.Match(salita, "[aeiou](ng|[bkdghlmnprstwy])[aeiou]$", RegexOptions.IgnoreCase).Value; // bATA, sITA, dATI, dULO, pUSO, sULO, sILO, gUPO, mATA, kUHA, daITI, dayAPA
            string hulingPPK = Regex.Match(salita, "[aeiou]{2}(ng|[bkdghlmnprstwy])$", RegexOptions.IgnoreCase).Value; // sUONG, dAONG, kAONG, dAING, hAIN, lAING, sAID, dIIN, dIIT, dUOP, gAOD
            string hulingPP = Regex.Match(salita, "[aeiou]{2}$", RegexOptions.IgnoreCase).Value; // bUO, bAO, nOO, yAO, pAA
            bool simulaSaD = Regex.IsMatch(salita, "^d", RegexOptions.IgnoreCase); 
            bool simulaSaKa = Regex.IsMatch(salita, "^k", RegexOptions.IgnoreCase);
            bool simulaSaNga = Regex.IsMatch(salita, "^ng", RegexOptions.IgnoreCase);
            bool simulaSaDARA = Regex.IsMatch(salita, "^d[aeiou]r[aeiou]");
            bool simulaSaPatinig = Regex.IsMatch(salita, "^[aeiou]", RegexOptions.IgnoreCase);
            bool taposSaD = Regex.IsMatch(salita, "d$", RegexOptions.IgnoreCase);
            bool taposSaPatinig = Regex.IsMatch(salita, "[aeiou]$", RegexOptions.IgnoreCase);
            bool taposSaPKPK = Regex.IsMatch(salita, "([aeiou](ng|[bkdghlmnprstwy])){2}$", RegexOptions.IgnoreCase);
            bool taposSaPKP = Regex.IsMatch(salita, "[aeiou](ng|[bkdghlmnprstwy])[aeiou]$", RegexOptions.IgnoreCase);
            bool taposSaPPK = Regex.IsMatch(salita, "[aeiou]{2}(ng|[bkdghlmnprstwy])$", RegexOptions.IgnoreCase);
            bool taposSaPP = Regex.IsMatch(salita, "[aeiou]{2}$", RegexOptions.IgnoreCase);
            bool panlapingTaposSaG = Regex.IsMatch(panlapi, "g$", RegexOptions.IgnoreCase);
            bool panlapingTaposSaNG = Regex.IsMatch(panlapi, "ng$", RegexOptions.IgnoreCase);
            bool panlapingTaposSaPatinig = Regex.IsMatch(panlapi, "[aeiou]$", RegexOptions.IgnoreCase);
            bool gitlingan = panlapingTaposSaG && simulaSaPatinig && panlapiUri == PanlapiUri.Unlapi;
            bool simulaSaBaPa = Regex.IsMatch(salita, "^[bp]", RegexOptions.IgnoreCase);
            bool simulaSaDaSaTa = Regex.IsMatch(salita, "^[dst]", RegexOptions.IgnoreCase);
            bool simulaSaLa = Regex.IsMatch(salita, "^l", RegexOptions.IgnoreCase);
            bool simulaSaDaLaSaTa = simulaSaLa || simulaSaDaSaTa;
            bool panlapingTaposSaNG2 = panlapingTaposSaNG && panlapiUri == PanlapiUri.Unlapi;
            bool panlapingTaposSaPatinig2 = panlapingTaposSaPatinig && panlapiUri == PanlapiUri.Unlapi;
            bool simulaSaBaDaLaPaSaTa = simulaSaBaPa || simulaSaDaLaSaTa;
            bool simulaSaBaDaPaSaTa = simulaSaBaPa || simulaSaDaSaTa;
            bool simulaSaLaWaYa = Regex.IsMatch(salita, "^[lwy]", RegexOptions.IgnoreCase);
            string panlapingNG = string.Empty;
            string gitlapiangKP = string.Empty;

            if (!taposSaPatinig)
            {
                if (salitaDiin == SalitaDiin.Malumi) salitaDiin = SalitaDiin.Malumay;
                else if (salitaDiin == SalitaDiin.Maragsa) salitaDiin = SalitaDiin.Mabilis;
            }

            if (panlapiUri == PanlapiUri.Wala)
            {
                banghay = banghay.Concat(new string[] { salita }).ToArray();
                if (ulitinAngUnangKP)
                {
                    banghay = banghay.Concat(new string[] { unangKP + salita }).ToArray();
                    if (simulaSaD && !simulaSaDARA && gawingRAngIkalawangUlit)
                    {
                        banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                    }
                }
            }
            else if (panlapiUri == PanlapiUri.Unlapi)
            {
                if (panlapingTaposSaNG2)
                {
                    if (simulaSaBaPa) panlapingNG = Regex.Replace(panlapi, "ng$", "m");
                    else if (simulaSaDaLaSaTa) panlapingNG = Regex.Replace(panlapi, "ng$", "n");
                    else panlapingNG = panlapi;
                }

                banghay = banghay.Concat(new string[] { panlapi + (gitlingan ? "-" : string.Empty) + salita }).ToArray();

                if (panlapingTaposSaNG2)
                {
                    if (simulaSaBaDaLaPaSaTa || simulaSaPatinig) banghay = banghay.Concat(new string[] { panlapingNG + salita }).ToArray();
                    if ((simulaSaBaDaPaSaTa || simulaSaKa) && !simulaSaPatinig)
                    {
                        banghay = banghay.Concat(new string[] { Regex.Replace(panlapingNG, "[mn]$", string.Empty, RegexOptions.IgnoreCase) + Regex.Replace(salita, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase) }).ToArray();
                    }
                }

                if (gawingRAngUnangTitik && simulaSaD && panlapingTaposSaPatinig2 && !simulaSaDARA)
                {
                    banghay = banghay.Concat(new string[] { panlapi + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                }

                if (ulitinAngUnangKP)
                {
                    banghay = banghay.Concat(new string[] { panlapi + (gitlingan ? "-" : string.Empty) + unangKP + salita }).ToArray();
                    if (panlapingTaposSaNG2)
                    {
                        if (simulaSaBaDaLaPaSaTa) banghay = banghay.Concat(new string[] { panlapingNG + unangKP + salita }).ToArray();

                        if (simulaSaPatinig)
                        {
                            banghay = banghay.Concat(new string[] { Regex.Replace(panlapingNG, "ng$", string.Empty, RegexOptions.IgnoreCase) + "ng" + unangKP + "ng" + salita }).ToArray();
                        }
                        else if (simulaSaBaDaPaSaTa || simulaSaKa)
                        {
                            banghay = banghay.Concat(new string[] 
                            {
                                Regex.Replace(panlapingNG, "[mn]$", string.Empty, RegexOptions.IgnoreCase) + 
                                Regex.Replace(unangKP, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase) + 
                                Regex.Replace(salita, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase)
                            }).ToArray();
                        }
                    }
                    if (simulaSaD && !simulaSaDARA)
                    {
                        if (gawingRAngIkalawangUlit)
                        {
                            banghay = banghay.Concat(new string[] { panlapi + (gitlingan ? "-" : string.Empty) + unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                            if (panlapingTaposSaNG2)
                            {
                                banghay = banghay.Concat(new string[] { panlapingNG + unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                            }
                        }
                        if (gawingRAngUnangTitik && simulaSaD && panlapingTaposSaPatinig2)
                        {
                            banghay = banghay.Concat(new string[] { panlapi + Regex.Replace(unangKP, "^d", "r", RegexOptions.IgnoreCase) + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                        }
                    }
                }

                /*
                for (int a = 0; a < 2; a++) 
                {
                    if (!(a == 1 && !ulitinAngUnangKP)) 
                    {
                        banghay = banghay.Concat(new string[] { panlapi + (gitlingan ? "-" : string.Empty) + (a == 1 ? unangKP: string.Empty) + salita }).ToArray();
                    }

                    if (panlapingTaposSaNG2)
                    {
                        if (simulaSaBaDaLaPaSaTa || simulaSaPatinig) banghay = banghay.Concat(new string[] { panlapingNG + salita }).ToArray();
                        if ((simulaSaBaDaPaSaTa || simulaSaKa) && !simulaSaPatinig)
                        {
                            banghay = banghay.Concat(new string[] { Regex.Replace(panlapingNG, "[mn]$", string.Empty, RegexOptions.IgnoreCase) + Regex.Replace(salita, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase) }).ToArray();
                        }

                        if (simulaSaBaDaLaPaSaTa) banghay = banghay.Concat(new string[] { panlapingNG + unangKP + salita }).ToArray();

                        if (simulaSaPatinig)
                        {
                            banghay = banghay.Concat(new string[] { Regex.Replace(panlapingNG, "ng$", string.Empty, RegexOptions.IgnoreCase) + "ng" + unangKP + "ng" + salita }).ToArray();
                        }
                        else if (simulaSaBaDaPaSaTa || simulaSaKa) //pasubali: pangangayupapa -> ngayupapa o dayupapa?
                        {
                            banghay = banghay.Concat(new string[] 
                            {
                                Regex.Replace(panlapingNG, "[mn]$", string.Empty, RegexOptions.IgnoreCase) + 
                                Regex.Replace(unangKP, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase) + 
                                Regex.Replace(salita, "^.", Regex.Match(panlapingNG, "[mn]$", RegexOptions.IgnoreCase).Value, RegexOptions.IgnoreCase)
                            }).ToArray();
                        }
                    }
                }
                
                
                
                if (gawingRAngUnangTitik && simulaSaD && panlapingTaposSaPatinig2 && !simulaSaDARA)
                {
                    banghay = banghay.Concat(new string[] { panlapi + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                }
                if (simulaSaD && !simulaSaDARA)
                {
                    if (gawingRAngIkalawangUlit)
                    {
                        banghay = banghay.Concat(new string[] { panlapi + (gitlingan ? "-" : string.Empty) + unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                        if (panlapingTaposSaNG2)
                        {
                            banghay = banghay.Concat(new string[] { panlapingNG + unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                        }
                    }
                    if (gawingRAngUnangTitik && simulaSaD && panlapingTaposSaPatinig2)
                    {
                        banghay = banghay.Concat(new string[] { panlapi + Regex.Replace(unangKP, "^d", "r", RegexOptions.IgnoreCase) + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                    }
                }
                */
            }

            else if (panlapiUri == PanlapiUri.Gitlapi)
            {
                gitlapiangKP = unangKP.Insert(simulaSaPatinig ? 0 : (simulaSaNga ? 2 : 1), panlapi);

                for (int a = 0; a < 2; a++) 
                {
                    if (!(a == 1 && !ulitinAngUnangKP)) 
                    {
                        banghay = banghay.Concat(new string[] { Regex.Replace((a == 1 ? unangKP: string.Empty) + salita, "^" + unangKP, gitlapiangKP) }).ToArray();

                        if (a == 1 && simulaSaD && !simulaSaDARA && gawingRAngIkalawangUlit)
                        {
                            banghay = banghay.Concat(new string[] { gitlapiangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) }).ToArray();
                        }

                        if (panlapi == "in" && simulaSaLaWaYa)
                        {
                            banghay = banghay.Concat(new string[] { "ni" + (a == 1 ? unangKP: string.Empty) + salita }).ToArray();
                        }
                    }
                }
                
            }

            else if (panlapiUri == PanlapiUri.Hulapi)
            {
                salita = Regex.Replace(salita, hulingPK + "$", hulingPK = Regex.Replace(hulingPK, "o", "u", RegexOptions.IgnoreCase), RegexOptions.IgnoreCase);

                if (taposSaPatinig && (salitaDiin == SalitaDiin.Malumay || salitaDiin == SalitaDiin.Mabilis)) panlapi = "h" + panlapi;
                banghay = banghay.Concat(new string[] { salita + panlapi }).ToArray();
                if (taposSaD) banghay = banghay.Concat(new string[] { Regex.Replace(salita, "d$", "r", RegexOptions.IgnoreCase) + panlapi }).ToArray();

                if (taposSaPKPK)
                {
                    banghay = banghay.Concat(new string[] { Regex.Replace(salita, hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                }
                else if (taposSaPKP)
                {
                    banghay = banghay.Concat(new string[] { Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + panlapi }).ToArray();
                    if (taposSaPatinig && (salitaDiin == SalitaDiin.Malumi || salitaDiin == SalitaDiin.Maragsa))
                    {
                        banghay = banghay.Concat(new string[] { Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "-" + panlapi }).ToArray();
                        banghay = banghay.Concat(new string[] { Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "'" + panlapi }).ToArray();
                    }
                }
                else if (taposSaPPK)
                {
                    banghay = banghay.Concat(new string[] { Regex.Replace(salita, hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                    if (taposSaD) banghay = banghay = banghay.Concat(new string[] { Regex.Replace(Regex.Replace(salita, "d$", "r", RegexOptions.IgnoreCase), Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase) + "$", Regex.Replace(Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase), "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                }

                // putol -> putl

                if (ulitinAngUnangKP)
                {
                    banghay = banghay.Concat(new string[] { unangKP + salita + panlapi }).ToArray();

                    if (taposSaD) banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, "d$", "r", RegexOptions.IgnoreCase) + panlapi }).ToArray();

                    if (taposSaPKPK)
                    {
                        banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                    }
                    else if (taposSaPKP)
                    {
                        banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        if (taposSaPatinig && (salitaDiin == SalitaDiin.Malumi || salitaDiin == SalitaDiin.Maragsa))
                        {
                            banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "-" + panlapi }).ToArray();
                            banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "'" + panlapi }).ToArray();
                        }
                    }
                    else if (taposSaPPK)
                    {
                        banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        if (taposSaD) banghay = banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "d$", "r", RegexOptions.IgnoreCase), Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase) + "$", Regex.Replace(Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase), "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                    }

                    if (simulaSaD && !simulaSaDARA && gawingRAngIkalawangUlit)
                    {
                        banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        if (taposSaD) banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), "d$", "r", RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        if (taposSaPKPK)
                        {
                            banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        }
                        else if (taposSaPKP)
                        {
                            banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + panlapi }).ToArray();
                            if (taposSaPatinig && (salitaDiin == SalitaDiin.Malumi || salitaDiin == SalitaDiin.Maragsa))
                            {
                                banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "-" + panlapi }).ToArray();
                                banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), hulingPK + "$", string.Empty, RegexOptions.IgnoreCase) + "'" + panlapi }).ToArray();
                            }
                        }
                        else if (taposSaPPK)
                        {
                            banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), hulingPK + "$", Regex.Replace(hulingPK, "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                            if (taposSaD) banghay = banghay = banghay.Concat(new string[] { unangKP + Regex.Replace(Regex.Replace(Regex.Replace(salita, "^d", "r", RegexOptions.IgnoreCase), "d$", "r", RegexOptions.IgnoreCase), Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase) + "$", Regex.Replace(Regex.Replace(hulingPK, "d$", "r", RegexOptions.IgnoreCase), "[aeiou]", string.Empty, RegexOptions.IgnoreCase), RegexOptions.IgnoreCase) + panlapi }).ToArray();
                        }
                    }
                }
            }
            return banghay;
        }
        static string[] Banghay(
            string salita
            )
        {
            string[] banghay = new string[] { };
            string[] unlapi = new string[] { "mag", "nag", "pag", "mang", "nang", "pang", "ka", "ma", "na", "pa", "i" };
            string[] gitlapi = new string[] { "um", "in" };
            string[] hulapi = new string[] { "an", /*"in"*/ };

            banghay = banghay.Concat(Lapiin(salita)).ToArray();
            //foreach (var item in unlapi)
            //{
            //    banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Unlapi, SalitaDiin.Malumay, item)).ToArray();
            //}
            foreach (var item in gitlapi)
            {
                banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Gitlapi, SalitaDiin.Malumay, item)).ToArray();
            }
            //foreach (var item in hulapi)
            //{
            //    banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Hulapi, SalitaDiin.Malumi, item)).ToArray();
            //    banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Hulapi, SalitaDiin.Maragsa, item)).ToArray();
            //    banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Hulapi, SalitaDiin.Malumay, item)).ToArray();
            //    banghay = banghay.Concat(Lapiin(salita, PanlapiUri.Hulapi, SalitaDiin.Mabilis, item)).ToArray();
            //}

            return banghay;
        }

        static void Main(string[] args)
        {
            do
            {
                Console.Write("Magpasok ng isang salita: ");
                string salita = Console.ReadLine();

                //string[] ulunan = new string[5]
                //{
                //    "PANLAPI",
                //    "MALUMAY",
                //    "MALUMI",
                //    "MARAGSA",
                //    "MABILIS"
                //};

                //List<string[][]> hanay = new List<string[][]>();

                //Banghay banghay = new Banghay(salita);

                //foreach (var mayLapi in banghay.MgaMayLapi) 
                //{
                //    string[][] mgaSalita = new string[5][];

                //    mgaSalita[0] = new string[] { mayLapi.Panlapi };
                //    mgaSalita[1] = new string[] {  };
                //    mgaSalita[2] = new string[] {  };
                //    mgaSalita[3] = new string[] {  };
                //    mgaSalita[4] = new string[] {  };

                //    foreach (var anyo in mayLapi.MgaAnyo)
                //    {
                //        mgaSalita[1] = mgaSalita[1].Concat(new string[] { anyo }).ToArray();
                //        mgaSalita[2] = mgaSalita[2].Concat(new string[] { anyo }).ToArray();
                //        mgaSalita[3] = mgaSalita[3].Concat(new string[] { anyo }).ToArray();
                //        mgaSalita[4] = mgaSalita[4].Concat(new string[] { anyo }).ToArray();
                //    }

                //    hanay.Add(mgaSalita);
                //}

                //Regex rSimulaSaPatinig = new Regex("^[aeiou]");
                //bool simulaSaPatinig = rSimulaSaPatinig.IsMatch(salita);

                //Regex rUnangKatinigPatinig = new Regex("^(ng|[bkdghlmnprstwy])?[aeiou]");
                //string unangKatinigPatinig = rUnangKatinigPatinig.Match(salita).Value;

                //Regex rSimulaSaDa = new Regex("^" + unangKatinigPatinig);
                //string dTungoR = rSimulaSaDa.Replace(salita, "r");

                //string banghay1 = string.Empty, banghay2 = string.Empty;
                //string[][] mgaSalita = new string[5][];

                //banghay1 = "mag" + (simulaSaPatinig ? "-" : string.Empty) + salita;

                //mgaSalita[0] = new string[] { "mag" };
                //mgaSalita[1] = new string[] { banghay1 };
                //mgaSalita[2] = new string[] { banghay1 };
                //mgaSalita[3] = new string[] { banghay1 };
                //mgaSalita[4] = new string[] { banghay1 };

                //hanay.Add(mgaSalita);

                //banghay1 = "mag" + (simulaSaPatinig ? "-" : string.Empty) + unangKatinigPatinig + salita;
                //banghay2 = "mag" + (simulaSaPatinig ? "-" : string.Empty) + unangKatinigPatinig + dTungoR;

                //mgaSalita = new string[5][];

                //hanay.Add(new string[5][]
                //{
                //    new string[] { "ma" },
                //    new string[] { "maisip", "maisip-isip" },
                //    new string[] { "maisip", "maisip-isip" },
                //    new string[] { "maisip", "maisip-isip" },
                //    new string[] { "maisip" }
                //});

                //hanay.Add(new string[5][]
                //{
                //    new string[] { "ma" },
                //    new string[] { "maisip", "maisip-isip", "maisip-isip" },
                //    new string[] { "maisip", "maisip-isip", "maisip-isip" },
                //    new string[] { "maisip", "maisip-isip" },
                //    new string[] { "maisip" }
                //});

                //Console.WriteLine(LumikhaNgTsart(ulunan, hanay.ToArray()));

                foreach (var item in Banghay(salita))
                {
                    Console.WriteLine(item);
                }
            }
            while (Console.ReadKey().Key == ConsoleKey.Enter);
        }
    }
}
