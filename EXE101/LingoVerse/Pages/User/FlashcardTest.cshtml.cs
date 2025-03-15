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