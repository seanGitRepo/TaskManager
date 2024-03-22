using Microsoft.EntityFrameworkCore;
using TaskManager;
using System.Threading.Tasks;
using System.ComponentModel.Design.Serialization;
using System.Data;

var context = new TMDbContext();
var query = context.Tasks;

var runTime = false;

while (!runTime)
{
    Thread.Sleep(1000);
    ListTasks();
   
    if (Console.KeyAvailable)
    {
        char inputChar = Console.ReadKey(true).KeyChar;

        if (inputChar == 'a')
        {
            
            addTask();

        }else if(inputChar == 'd')
        {
            deleteTask();
        }

    }

}

// TODO: add
// TODO: delete
// TODO: edit
// TODO: Version History


void addTask()
{

    Console.Clear();
    Console.WriteLine($"\tTASK ADDER \t \t \t \t {DateTime.Now.ToLongTimeString()}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("\tName of Task:");
    string name = Console.ReadLine();
    Console.WriteLine();
    Console.WriteLine("\tDescription of Task:");
    string desc = Console.ReadLine();

    var add = new TaskManager.Task { Title = name, Description = desc };
    context.Tasks.Add(add);
    context.SaveChanges();
  

}

void deleteTask()
{

    Console.WriteLine("\tWhat task is to be delted? Please select a valid id:");
    int id = int.Parse(Console.ReadLine());


    var deletethismofo =  context.Tasks.FirstOrDefault(tasksearch => tasksearch.ID == id);
    context.Tasks.ExecuteDelete();

    context.SaveChanges();




}

 void ListTasks()
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine($"\tTASK MANAGER \t \t \t \t {DateTime.Now.ToLongTimeString()}");
    Console.WriteLine();
    Console.WriteLine();
    int count = 0;

    foreach (var item in query)
    {
        count ++;
        Console.WriteLine($"{count} \t Task: {item.Title}\n" +
            $" \t Description: {item.Description}" );

        Console.WriteLine();



    }

    Console.WriteLine($"\tAction Key:");
    Console.WriteLine($"\ta: add a new task");
    Console.WriteLine($"\td: delete a task:");
    Console.WriteLine($"\te: edit a task:");

}


//context.Tasks.Remove(entityToDelete);


//for (int i = 0; i < 1000; i++)
//    {
//                var task = context.Tasks.FirstOrDefault( t => t.ID == 1);


//    string x = "this is a test to see how the version control interacts. I don't believe it will change anything";

//        string[] splut = x.Split(' ');

//            task.Description = splut[i%17];

//            Console.WriteLine(task.Description);
//            Console.WriteLine(task.Version);
//            task.Version = Guid.NewGuid();
//            Console.ReadLine();


//            try
//            {
//                context.SaveChanges();
//                Console.WriteLine($"{i} yeet");
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//        Console.WriteLine("Concurrency conflict detected. Reloading entity and retrying...");
//        var entry = ex.Entries.Single();
//        var databaseValues = (TaskManager.Task)entry.GetDatabaseValues().ToObject();
//        var clientValues = (TaskManager.Task)entry.Entity;

//        Console.WriteLine($"{databaseValues.Version}: {databaseValues.Description}");
//        Console.WriteLine($"{clientValues.Version}: {clientValues.Description}");

//        Console.ReadLine();

//        // Update clientValues with databaseValues
//        databaseValues.Description = clientValues.Description;
//        databaseValues.Version = clientValues.Version;

//        // Retry the save operation
//        context.SaveChanges();
//        Console.WriteLine("Retry changes saved successfully.");
//    }




//    }


//TODO: history of transactions









