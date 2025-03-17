using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.User
{
    public class FlashcardMatchModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Type { get; set; } = "Line";

        public Dictionary<string, List<TermOption>> FinalListTest { get; set; }

        // Questions for left column
        public List<string> LeftColumnItems { get; set; } = new();

        // Answer pairs for right column (correct and incorrect answers)
        public List<string> RightColumnItems { get; set; } = new();

        public int Remaining { get; set; } = 1;
        public int Column { get; set; } = 4;
        public string Height { get; set; } = "120px";
        public string FontSize { get; set; } = "14";


        public void OnGet()
        {
            // Initialize test data
            InitializeTestData();

            // Setup left and right columns
            SetupColumns();

            // Adjust columns and sizing based on the number of items
            AdjustLayout();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            // Handle form submission (continue button)
            return RedirectToPage("/Index");
        }

        private void InitializeTestData()
        {
            FinalListTest = new Dictionary<string, List<TermOption>>();

            // Question 1
            var question1 = "Hello";
            var options1 = new List<TermOption>
        {
            new TermOption {
                Type = "Definition",
                IsCorrect = true,
                Term = new Term { TermName = "Xin chào", Definition = "Xin chào" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Tạm biệt", Definition = "Tạm biệt" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Từ chối", Definition = "Từ chối" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Chúc mừng", Definition = "Chúc mừng" }
            }
        };
            FinalListTest.Add(question1, options1);

            // Question 2
            var question2 = "Goodbye";
            var options2 = new List<TermOption>
        {
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "Tức giận", Definition = "Tức giận" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = true,
                Term = new Term { TermName = "Tạm biệt", Definition = "Tạm biệt" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "Vui vẻ", Definition = "Vui vẻ" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "Đi nào", Definition = "Đi nào" }
            }
        };
            FinalListTest.Add(question2, options2);

            // Question 3
            var question3 = "Thank you";
            var options3 = new List<TermOption>
        {
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Chào hỏi", Definition = "Chào hỏi" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = true,
                Term = new Term { TermName = "Cảm ơn", Definition = "Cảm ơn" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "hài lòng", Definition = "hài lòng" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Vui vẻ", Definition = "Vui vẻ" }
            }
        };
            FinalListTest.Add(question3, options3);
        }

        private void SetupColumns()
        {

            // Initialize right column with answer pairs (1 correct, 1 incorrect)
            foreach (var d in FinalListTest) 
            {
                LeftColumnItems.Add(d.Key);
                RightColumnItems.Add(d.Value.First(x => x.IsCorrect).Term.TermName);
            }
        }

        private void AdjustLayout()
        {
            // Set layout parameters based on number of items
            int size = LeftColumnItems.Count;

            if (Type == "Tiles" || Type == "Memory")
            {
                Column = 4;

                if (size <= 5)
                {
                    Height = "120px";
                    FontSize = "14";
                }
                else if (size == 6)
                {
                    Height = "100px";
                    FontSize = "14";
                }
                else if (size == 7)
                {
                    Height = "90px";
                    FontSize = "14";
                }
                else if (size == 8)
                {
                    Height = "70px";
                    FontSize = "12";
                }
                else if (size == 9)
                {
                    Height = "60px";
                    FontSize = "12";
                }
                else
                {
                    Height = "50px";
                    FontSize = "11";
                }
            }
        }
    }
}
