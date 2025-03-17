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
                    "Hello", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 1, Definition = "Xin chào", IsCorrect = true },
                        new TermDefinition { TermId = 2, Definition = "Tạm biệt", IsCorrect = false },
                        new TermDefinition { TermId = 3, Definition = "Từ chối", IsCorrect = false },
                        new TermDefinition { TermId = 4, Definition = "Chúc mừng", IsCorrect = false }
                    }
                },
                {
                    "Goodbye", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 5, Definition = "Tức giận", IsCorrect = false },
                        new TermDefinition { TermId = 6, Definition = "Tạm biệt", IsCorrect = true },
                        new TermDefinition { TermId = 7, Definition = "Vui vẻ", IsCorrect = false },
                        new TermDefinition { TermId = 8, Definition = "Đi nào", IsCorrect = false }
                    }
                },
                {
                    "Thank you", new List<TermDefinition>
                    {
                        new TermDefinition { TermId = 9, Definition = "Chào hỏi", IsCorrect = false },
                        new TermDefinition { TermId = 10, Definition = "Cảm ơn", IsCorrect = true },
                        new TermDefinition { TermId = 11, Definition = "hài lòng", IsCorrect = false },
                        new TermDefinition { TermId = 12, Definition = "Vui vẻ", IsCorrect = false }
                    }
                },
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
