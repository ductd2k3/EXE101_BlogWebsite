using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

public class FlashCardTestModel : PageModel
{
    public int Total { get; set; }
    public int Remaining { get; set; }
    public string SetId { get; set; }
    public Dictionary<string, List<TermOption>> FinalListTest { get; set; }

    public int TimeInMinutes { get; set; } = 20;
    public void OnGet(string setId)
    {
        // Set temporary data
        SetId = setId ?? "1";
        Total = 10;
        Remaining = 5;

        // Create sample test data
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

        // Add more sample questions as needed...
    }

    public IActionResult OnPostResult(string answers, string setId)
    {
        // Process the answers and redirect to results page
        return RedirectToPage("ResultTest", new { answers = answers, setId = setId });
    }
}

// Models for the page
public class Term
{
    public string TermName { get; set; }
    public string Definition { get; set; }
}

public class TermOption
{
    public string Type { get; set; } // "Definition" or "Term"
    public bool IsUserChoose { get; set; }
    public bool IsCorrect { get; set; }
    public Term Term { get; set; }
}