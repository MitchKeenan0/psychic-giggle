using System;
using System.IO;
using System.Reflection;

namespace QA_ENGNR_GetLeastRepeatingNumberFromFileTxt
{
	internal class Program
	{
		

		static void Main(string[] args)
		{
			var rd = new Program();

			// create a string variable and get user input from the keyboard and store it in the variable
			Console.WriteLine("Enter File or Folder to Read: ");
			string readName = Console.ReadLine();

			// discern file from folder and go
			if (File.Exists(readName))
				rd.ReadFile(readName);
			else if (Directory.Exists(readName))
				rd.ReadFolder(readName);
		}

		void ReadFolder(string folderPath)
		{
			// assuming all files in directory are project safe
			string[] files = Directory.GetFiles(folderPath);
			foreach (string file in files)
				ReadFile(file);
		}

		void ReadFile(string filePath)
		{
			if (File.Exists(filePath))
			{
				// transmute file lines into number array
				string[] lines = File.ReadAllLines(filePath);
				int[] numberArray = NumberArrayFromTextLines(lines);
				if (numberArray.Length > 0)
				{
					// calculate least repeating number
					List<int>? leastRepeatingNumberList = GetLeastRepeatingNumber(numberArray);
					if (leastRepeatingNumberList != null)
					{
						// output
						string fileNameClean = Path.GetFileName(filePath);
						int number = leastRepeatingNumberList[0];
						int repeats = leastRepeatingNumberList.Count;

						Console.WriteLine("File: {0}, Number: {1}, Repeated: {2} times",
							fileNameClean, number, repeats);
					}
				}
			}
		}

		int[] NumberArrayFromTextLines(string[] textLines)
		{
			int numLines = textLines.Length;
			List<int> numberList = new List<int>();
			for (int i = 0; i < numLines; i++)
			{
				if (Int32.TryParse(textLines[i], out int j))
				{
					numberList.Add(j);
				}
			}

			return numberList.ToArray();
		}

		List<int>? GetLeastRepeatingNumber(int[] numberArray)
		{
			// store 'a list of lists' to tally repeating numbers
			List<List<int>> superList = new List<List<int>>();

			// loop each number and stack le stacks
			int numNumbers = numberArray.Length;
			for (int i = 0; i < numNumbers; i++)
			{
				int number = numberArray[i];

				// check if there's already a list of this number
				List<int>? repeatingList = NumberListedInSuperList(number, superList);
				if (repeatingList != null)
				{
					repeatingList.Add(number);
				}
				else
				{
					// not yet listed - create new list
					List<int> newList = new List<int>();
					newList.Add(number);

					// add to super list
					superList.Add(newList);
				}
			}

			// tally completed, get least numerous list
			List<int>? smallestList = GetShortestList(superList);
			if (smallestList != null)
			{
				// get other lists with matching count
				List<List<int>> smallestSuperList = new List<List<int>>();
				smallestSuperList.Add(smallestList);
				foreach(List<int> list in superList)
				{
					if (list.Count == smallestList.Count)
						smallestSuperList.Add(list);
				}

				// early exit for singularly fewest repeated number
				if (smallestSuperList.Count == 1)
					return smallestSuperList[0];

				//--------------

				// if multiple lists share smallest number, return lowest number
				int lowestNumber = int.MaxValue;
				List<int>? lowestList = null;
				foreach(List<int> list in smallestSuperList)
				{
					int number = list[0];
					if (number < lowestNumber)
					{
						lowestNumber = number;
						lowestList = list;
					}
				}

				// least repeated, smallest number
				if (lowestList != null)
				{
					return lowestList;
				}
			}

			return null;
		}

		List<int>? NumberListedInSuperList(int number, List<List<int>> superList)
		{
			foreach(List<int> list in superList)
			{
				if ((list.Count > 0) && (list[0] == number))
					return list;
			}

			return null;
		}

		List<int>? GetShortestList(List<List<int>> superList)
		{
			int leastCount = int.MaxValue;
			List<int>? smallestList = null;

			foreach(List<int> list in superList)
			{
				int count = list.Count;
				if (count < leastCount)
				{
					leastCount = count;
					smallestList = list;
				}
			}

			return smallestList;
		}
	}
}