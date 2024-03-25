using Microsoft.EntityFrameworkCore;
using TaskManager;
using System.Threading.Tasks;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Threading.Channels;

var context = new TMDbContext();
var query = context.Tasks;
int count = 0;


var runTime = false;

while (!runTime)
{

    count++;

    if (count%5 == 1)
    {
        
        context = new TMDbContext();
        query = context.Tasks;
    }
    
    Thread.Sleep(1000);
    ListTasks();
   
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo menuPress = Console.ReadKey(true);

        if (menuPress.KeyChar == 'a')
        {
            
            addTask();

        }else if(menuPress.KeyChar == 'd')
        {
            deleteTask();
        }else if(menuPress.Key == ConsoleKey.Escape)
        {
            Environment.Exit(0);

        }else if(menuPress.KeyChar == 'e')
        {

            editTask(); 
        }

    }

}


// TODO: edit
// TODO: Version History
// something really funny, is that i had    context.Tasks.ExecuteDelete();which delted my entire original database.


void editTask()  
{

    Console.WriteLine();


    Console.WriteLine($"\tWhat task is to be edited? \t \t \t \t {DateTime.Now.ToLongTimeString()} " +
        "\nPlease select a valid id: ");
    int id = -1;
    try
    {
        id = int.Parse(Console.ReadLine());
    }
    catch (Exception)
    {


        Console.WriteLine("This user was not found");

    }


    if (id != -1)
    {
        var editThisMofo = context.Tasks.FirstOrDefault(tasksearch => tasksearch.ID == id);
        
        if (editThisMofo != null)
        {
            Console.Clear();
            Console.WriteLine($"\tEdit Screen \t \t \t \t {DateTime.Now.ToLongTimeString()} ");
            Console.WriteLine("Name: {0}",editThisMofo.Title); //curent task info goes here.
            Console.WriteLine( );
            Console.WriteLine("Description: {0}", editThisMofo.Description);
            Console.WriteLine(  );
            Console.WriteLine("Database Version: {0}",editThisMofo.Version);// this is the guid version the database has
        
            Console.WriteLine();


            editThisMofo.Version = Guid.NewGuid(); // set a new task for this "action" on the database, this is set in the ram.
             Console.WriteLine("RAM Version: {0}",editThisMofo.Version); // this the version of row that is currently in the RAM of the file.

            //Here I need to accept the changes to the existing task.

            Console.WriteLine();
            Console.WriteLine("Enter the new Title, type \"keep\" to use old Title");
            string choiceT = Console.ReadLine();

            if (choiceT == "keep" )
            {

            }
            else
            {
                editThisMofo.Title = choiceT;
            }
            Console.WriteLine();
            Console.WriteLine("Enter the new Desc, type \"keep\" to use old Title");
            string choiceD = Console.ReadLine();

            if (choiceD == "keep")
            {

            }
            else
            {
                editThisMofo.Description = choiceD;
            }

            Console.Clear();

            Console.WriteLine("\tFor demonstration purposes:");
            Console.WriteLine();
            Console.WriteLine("The program has not yet saved the updates to the database and is currently sitting in the RAM");
            Console.WriteLine("When I press enter, the program will send the DDL query to the database, checking if the guid version numbers");
            Console.WriteLine("are still matching. If they are, it will save the database succesfully.If the program detects a difference in numbers");
            Console.WriteLine("Can someone tell me at this point what that means? and what the application might do ?");
            Console.ReadLine();
            Console.WriteLine("Yep, it will throw an error as someone else in a differnt application has changed that row since you begun ur edit");
            Console.WriteLine("and they have sucesffuly made a save to the database");
            Console.ReadLine();

            Console.Clear();
            try
            {
                context.SaveChanges();
              
            }
            catch (DbUpdateConcurrencyException ex)
            {



                Console.WriteLine("Concurrency conflict detected. Reloading entity and retrying...");

                foreach (var taskSearch in ex.Entries) 
                {
                    if (taskSearch.Entity is TaskManager.Task) 
                    {
                        var proposedvalues = taskSearch.CurrentValues;
                        var databasevalues = taskSearch.GetDatabaseValues();

                        string propTitle = "";
                        string propDesc = "";
                        string dataTitle = "";
                        string dataDesc = "";


                        foreach (var item in proposedvalues.Properties)
                        {

                            if (item.Name == "Title")
                            {
                                propTitle = proposedvalues[item].ToString();
                                dataTitle = databasevalues[item].ToString();
                            }else if (item.Name == "Description")
                            {

                                propDesc = proposedvalues[item].ToString();
                                dataDesc = databasevalues[item].ToString();
                            }else if(item.Name == "Version")
                            {
                                
                                proposedvalues[item] = Guid.NewGuid();
                               

                            }

                           
                        }


                        Console.WriteLine($"1.\t Different User: {dataTitle}: {dataDesc}");
                        Console.WriteLine($"2.\t What you have changed: {propTitle}: {propDesc}");
                        Console.WriteLine();
                        Console.WriteLine("Which would you like to commit to the database ?");

                        string choiceC = Console.ReadLine();

                        if (choiceC == "1")
                        {
                          
                            taskSearch.OriginalValues.SetValues(databasevalues);
                        }
                        else if (choiceC == "2")
                        {

                            taskSearch.OriginalValues.SetValues(proposedvalues);
                        }

                    }
                }

                try
                {
                    context.SaveChanges();
                    Console.WriteLine("Changes saved successfully.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error saving changes.");
                  
                }
            }


        }
        else
        {

            Console.WriteLine("This user was not found");
        }


        

    }

  



}

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

    Guid v = Guid.NewGuid();

    var add = new TaskManager.Task { Title = name, Description = desc, Version = v };
    context.Tasks.Add(add);
    context.SaveChanges();
  

}

void deleteTask()
{
    Console.WriteLine();

    Console.WriteLine($"\tWhat task is to be delted? \t \t \t \t {DateTime.Now.ToLongTimeString()} ");
      Console.WriteLine("Please select a valid id: ");
    int id = -1;
    try
    {
        id = int.Parse(Console.ReadLine());
    }
    catch (Exception)
    {


        Console.WriteLine("This user was not found");

    }
    
    if (id != -1)
    {
        var deletethismofo = context.Tasks.FirstOrDefault(tasksearch => tasksearch.ID == id);

        if (deletethismofo != null)
        {


            context.Tasks.Remove(deletethismofo);
        }
        else
        {

            Console.WriteLine("This user was not found");
        }


        context.SaveChanges();

    }
   
    


}

 void ListTasks()
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine($"\tTASK MANAGER \t \t \t \t {DateTime.Now.ToLongTimeString()}");
    Console.WriteLine();
    Console.WriteLine($"ID \t Task Name");
    Console.WriteLine($"--- \t----------");
    Console.WriteLine();
  

    foreach (var item in context.Tasks)
    {
       
        Console.WriteLine($"{item.ID} \t Task: {item.Title}\n" +
            $" \t Description: {item.Description}" );

        Console.WriteLine();



    }

    Console.WriteLine($"\tAction Key:");
    Console.WriteLine($"\ta: add a new task");
    Console.WriteLine($"\td: delete a task:");
    Console.WriteLine($"\te: edit a task:");
    Console.WriteLine($"\tesc: close application:");

}




//TODO: history of transactions









