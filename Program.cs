using Big_company_problem;
using System.Linq;
using System.Reflection.Emit;

var conflictsBase = new ConflictsDataBase<string>();

var Vasya = "Vasya";
var Petya = "Petya";
var Alena = "Alena";
var Regina = "Regina";
var Roma = "Roma";
var Kolya = "Kolya";

List<string> all = new List<string>() { Vasya, Petya, Alena, Regina, Roma, Kolya };

conflictsBase.AddConflict(Vasya, Regina);
conflictsBase.AddConflict(Petya, Alena);
conflictsBase.AddConflict(Regina, Roma);
conflictsBase.AddConflict(Kolya, Vasya);
conflictsBase.AddConflict(Kolya, Alena);

//var a = "A";
//var b = "B";
//var c = "C";
//var d = "D";

//List<string> all = new() { a, b, c, d };

//conflictsBase.AddConflict(a, b);
//conflictsBase.AddConflict(b, c);
//conflictsBase.AddConflict(c, a);

List<List<string>> stableCompanies = GetAllStableCompanies(conflictsBase, all);

// Printing
if(stableCompanies.Count > 0)
{    
    int lastScCount = stableCompanies[0].Count;

    foreach(List<string> sc in stableCompanies)
    {
        if(sc.Count < lastScCount)
        {
            lastScCount = sc.Count;
            Console.WriteLine();
        }

        Console.Write($"{sc.Count}: ");
        WriteList(sc);
    }
}

static List<List<T>> GetAllStableCompanies<T>(ConflictsDataBase<T> conflictsDataBase, List<T> subjects)
{
    List<List<T>> stables = new();
    List<T> kicked = new();

    FindStable(subjects, new List<T>());

    // Postprocessing
    if(stables.Count > 0)
    {
        stables.Sort((a, b) => b.Count - a.Count);

        List<List<T>> elementsToRemoving = new();

        for(int i = stables.Count - 1; i > 0; i--)
        {
            for(int j = i - 1; j >= 0; j--)
            {
                var intersection = stables[i].Intersect(stables[j]);

                if(intersection.Count() == stables[i].Count)
                {
                    elementsToRemoving.Add(stables[i]);
                    break;
                }
            }
        }

        foreach(List<T> elementToRemoving in elementsToRemoving)
            stables.Remove(elementToRemoving);
    }

    return stables;

    void FindStable(List<T> subjects, List<T> saved)
    {
        for(int i = 0; i < subjects.Count; i++)
        {
            for(int j = i; j < subjects.Count; j++)
            {
                if(conflictsDataBase.IsHaveAnyConflict(subjects[i], subjects[j]))
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //WriteList(subjects);
                    //Console.ResetColor();

                    (List<T> list1, List<T> list2, List<T> list3) = SplitSubjects(subjects, subjects[i], subjects[j]);

                    int c = 0;

                    if(!saved.Contains(subjects[i]))
                    {
                        c++;
                        List<T> nextsaved = new List<T>(saved) { subjects[j] };
                        FindStable(list1, nextsaved);
                    }

                    if(!saved.Contains(subjects[j]))
                    {
                        c++;
                        List<T> nextsaved = new List<T>(saved) { subjects[i] };
                        FindStable(list2, nextsaved);
                    }

                    if(c == 2)
                        FindStable(list3, saved);

                    return;
                }
            }
        }

        //WriteList(subjects);
        stables.Add(subjects);
    }
}

static (List<T>, List<T>, List<T>) SplitSubjects<T>(List<T> subjects, T s1, T s2)
{
    List<T> l1 = new(subjects.Count);
    List<T> l2 = new(subjects.Count);
    List<T> l3 = new(subjects.Count);

    Action<List<T>, T> CreateCopier(T subject)
    {
        Action<List<T>, T> copier = (l, s) =>
        {
            if(s.Equals(subject))
            {
                copier = (a, b) => a.Add(b);
                return;
            }

            l.Add(s);
        };

        return copier;
    }

    Action<List<T>, T> CopyExS1 = CreateCopier(s1);
    Action<List<T>, T> CopyExS2 = CreateCopier(s2);

    foreach(T subject in subjects)
    {
        CopyExS1(l1, subject);
        CopyExS2(l2, subject);

        if(!subject.Equals(s1) && !subject.Equals(s2))
            l3.Add(subject);
    }

    return (l1, l2, l3);
}

static void WriteList<T>(List<T> list)
{
    if(list.Count != 0)
    {
        int i = 0;

        for(; i < list.Count - 1; i++)
            Console.Write($"{list[i]}, ");

        Console.WriteLine($"{list[i]}");
    }
}
