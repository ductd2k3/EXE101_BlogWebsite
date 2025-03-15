using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LingoVerse.Pages.User
{
    public class FlashCardLearnModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int SetId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Action { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Question { get; set; }

        public KeyValuePair<string, List<TermDefinition>> CurrentSet { get; set; }

        public string encodeQueryQuestion { get; set; }
        public int Remaining { get; private set; }
        public int Total { get; private set; }

        // Temporary data to simulate session storage
        private static Dictionary<string, List<TermDefinition>> finalListTest;
        private static Dictionary<string, List<TermDefinition>> allFlashcards;

        public FlashCardLearnModel()
        {
            // Initialize test data if not already done
            if (allFlashcards == null)
            {
                InitializeTestData();
            }
        }

        private void InitializeTestData()
        {
            // Create some sample flashcard data
            allFlashcards = new Dictionary<string, List<TermDefinition>>
            {
                {
                    "What is a programming language?", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 1, Definition = "A formal language that specifies a set of instructions for a computer", IsCorrect = true },
                        new TermDefinition { TermId = 2, Definition = "A tool used to design user interfaces", IsCorrect = false },
                        new TermDefinition { TermId = 3, Definition = "A document that outlines program requirements", IsCorrect = false },
                        new TermDefinition { TermId = 4, Definition = "A database management system", IsCorrect = false }
                    }
                },
                {
                    "What is C#?", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 5, Definition = "A programming language developed by Microsoft", IsCorrect = true },
                        new TermDefinition { TermId = 6, Definition = "A database query language", IsCorrect = false },
                        new TermDefinition { TermId = 7, Definition = "A music note", IsCorrect = false },
                        new TermDefinition { TermId = 8, Definition = "A markup language", IsCorrect = false }
                    }
                },
                {
                    "What is ASP.NET?", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 9, Definition = "A web framework developed by Microsoft", IsCorrect = true },
                        new TermDefinition { TermId = 10, Definition = "A programming language", IsCorrect = false },
                        new TermDefinition { TermId = 11, Definition = "A database system", IsCorrect = false },
                        new TermDefinition { TermId = 12, Definition = "An operating system", IsCorrect = false }
                    }
                },
                {
                    "What is HTML?", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 13, Definition = "A markup language for creating web pages", IsCorrect = true },
                        new TermDefinition { TermId = 14, Definition = "A programming language", IsCorrect = false },
                        new TermDefinition { TermId = 15, Definition = "A database system", IsCorrect = false },
                        new TermDefinition { TermId = 16, Definition = "A web server", IsCorrect = false }
                    }
                },
                {
                    "What is CSS?", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 17, Definition = "A style sheet language for describing the presentation of a document", IsCorrect = true },
                        new TermDefinition { TermId = 18, Definition = "A programming language", IsCorrect = false },
                        new TermDefinition { TermId = 19, Definition = "A security protocol", IsCorrect = false },
                        new TermDefinition { TermId = 20, Definition = "A database management system", IsCorrect = false }
                    }
                }
            };

            // Clone the original flashcard data to use for testing sessions
            finalListTest = new Dictionary<string, List<TermDefinition>>(allFlashcards);
            Total = finalListTest.Count;
        }

        public IActionResult OnGet()
        {
            // For initial load
            Remaining = finalListTest.Count;
            Total = allFlashcards.Count;
            if (Action == "skip" && !string.IsNullOrEmpty(Question))
            {
                // Move the current question to the end of the list
                var skippedSet = finalListTest[Question];
                finalListTest.Remove(Question);
                finalListTest.Add(Question, skippedSet);
                return RedirectToPage("/FlashLearn", new { setId = SetId });
            }
            else if (Action == "continue" && !string.IsNullOrEmpty(Question))
            {
                // Remove the answered question from the list
                finalListTest.Remove(Question);

                // If no more questions, prepare to show the completion modal
                if (finalListTest.Count == 0)
                {
                    // Reset for a new session
                    InitializeTestData();
                    return RedirectToPage("FlashCardLearn", new { setId = SetId, completed = true });
                }
                return RedirectToPage("FlashCardLearn", new { setId = SetId, completed = true });
            }
            else if (Action == "displayFlashcard")
            {
                InitializeTestData();
                return RedirectToPage("FlashcardList");
            }

            // Get the first question from the remaining list
            if (finalListTest.Count > 0)
            {
                CurrentSet = finalListTest.First();
                encodeQueryQuestion = System.Web.HttpUtility.UrlEncode(CurrentSet.Key); ;
            }

            return Page();
        }
    }

    public class TermDefinition
    {
        public int TermId { get; set; }
        public string Definition { get; set; }
        public bool IsCorrect { get; set; }

        // For compatibility with the original JSP model
        public Term GetTerm()
        {
            return new Term { Definition = Definition };
        }
    }
}
