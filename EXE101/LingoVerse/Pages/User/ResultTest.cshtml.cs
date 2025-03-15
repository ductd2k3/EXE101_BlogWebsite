using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.User
{
    public class ResultTestModel : PageModel
    {
        // Properties to hold data for the view
        public Dictionary<string, List<TermOption>> FinalListTest { get; set; }
        public int NumberOfCorrect { get; set; }
        public string SetId { get; set; }
        public string SearchBox { get; set; }
        public string SearchSelect { get; set; }
        public string Answers { get; set; }

        public void OnGet(string setId)
        {
            Answers = TempData["Answers"] as string; // Lấy lại giá trị
            // In a real application, this data would be loaded from a service
            SetId = setId;
            FinalListTest = new Dictionary<string, List<TermOption>>();
            // Question 1
            var question1 = "What is the capital of France?";
            var options1 = new List<TermOption>
        {
            new TermOption {
                Type = "Definition",
                IsCorrect = true,
                Term = new Term { TermName = "Paris", Definition = "Paris" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "London", Definition = "London" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Berlin", Definition = "Berlin" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Madrid", Definition = "Madrid" }
            }
        };
            FinalListTest.Add(question1, options1);

            // Question 2
            var question2 = "What is 2 + 2?";
            var options2 = new List<TermOption>
        {
            new TermOption {
                Type = "Term",
                IsCorrect = true,
                Term = new Term { TermName = "4", Definition = "Four" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "3", Definition = "Three" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "5", Definition = "Five" }
            },
            new TermOption {
                Type = "Term",
                IsCorrect = false,
                Term = new Term { TermName = "6", Definition = "Six" }
            }
        };
            FinalListTest.Add(question2, options2);

            // Question 3
            var question3 = "What is H2O?";
            var options3 = new List<TermOption>
        {
            new TermOption {
                Type = "Definition",
                IsCorrect = true,
                Term = new Term { TermName = "Water", Definition = "Water" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Air", Definition = "Air" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Fire", Definition = "Fire" }
            },
            new TermOption {
                Type = "Definition",
                IsCorrect = false,
                Term = new Term { TermName = "Earth", Definition = "Earth" }
            }
        };
            FinalListTest.Add(question3, options3);
            if(Answers != null)
            {

            FinalListTest = SetUserChooseAnswer(FinalListTest, Answers);
            }
            NumberOfCorrect = FinalListTest.Values.SelectMany(termOptions => termOptions) .Count(option => option.IsCorrect && option.IsUserChoose);
        }

        public static Dictionary<string, List<TermOption>> SetUserChooseAnswer(Dictionary<string, List<TermOption>> finalListTest, string answers)
        {
            var answerList = answers.Split(',').Select(item => item.Split(new[] { "-", "_" }, StringSplitOptions.RemoveEmptyEntries)).ToList();

            int countQuestion = 1;
            foreach (var entry in finalListTest)
            {
                foreach (var path in answerList)
                {
                    if (path.Length < 4) continue; // Bỏ qua nếu không đủ dữ liệu

                    int question = int.Parse(path[1]);
                    int answerIndex = int.Parse(path[3]);

                    if (question == countQuestion)
                    {
                        if (answerIndex > 0 && answerIndex <= entry.Value.Count)
                        {
                                entry.Value[answerIndex - 1].IsUserChoose = true;
                        }
                    }
                }
                countQuestion++;
            }
            return finalListTest;
        }

        public IActionResult OnPostSearch(string searchSelect, string searchBox, string answers, string setId)
        {
            SetId = setId;
            SearchBox = searchBox;
            SearchSelect = searchSelect;
            Answers = answers;

            return Page();
        }

        public IActionResult OnPost(string answers,int setId)
        {
            Answers = answers;
            TempData["Answers"] = answers; // Lưu dữ liệu tạm
            return RedirectToPage(setId);
        }

    }

}