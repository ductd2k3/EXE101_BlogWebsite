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