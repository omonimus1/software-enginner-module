# software-enginner-module
Exercises and projects created during the software-enginner-module at Napier University, using C#. 

# Week 1 #

The project of the week 1 is a WPF application with the following layout:
[!Project 1](media/project1.png)

All the text fiels must be validated in the following way: 
* Name: required (not blank)
* Age: in a range between 0 and 100 (inclusive)
* Address: required (not blank)

Once validated the name, age and address should be appended to a comma-separated text file (.csv)- 
see then example code at the end to demonstrate how to write to a text file.
Pressing the Clear button should remove the contents of the 3 text boxes.

. If you have finished task 1, amend your application as follows; firstly add a load button. 
When the load button is pressed load the names, ages and addresses from your CSV file. Now add a 
button called Next, when the next button is clicked show the next set of details in the file. HINT: 
You will need to perform some web searches in order to find out how to read a text file.

```
// A string of data
string line = "This will be appended to a file";
// Write the string to a file.
System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt",true); file.WriteLine(lines);
file.Close();
```