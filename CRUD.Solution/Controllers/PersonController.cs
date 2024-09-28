using CRUD.ServiceContracts;
using CRUD.ServiceContracts.DTO.Enums;
using CRUD.ServiceContracts.DTO.PersonDTO;
using Microsoft.AspNetCore.Mvc;
using CRUD.ServiceContracts.DTO.PersonDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD.Solution.Controllers;

public class PersonController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    public PersonController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }

    public IActionResult Index(string? searchBy, string? searchString, string sortBy = nameof(PersonResponse.Name),
        SortOrderOption sortOrder = SortOrderOption.ASC)
    {
        ViewBag.SearchFilds = new Dictionary<string, string>()
        {
            { nameof(PersonResponse.Name), "Name" },
            { nameof(PersonResponse.Email), "Email" },
            { nameof(PersonResponse.DateOfBirth), "Date of birth" },
            { nameof(PersonResponse.Gender), "Gender" },
            { nameof(PersonResponse.CountryName), "Country" },
            { nameof(PersonResponse.Address), "Address" },
        };
        ViewBag.searchBy = searchBy;
        ViewBag.searchString = searchString;
        ViewBag.sortBy = sortBy;
        ViewBag.sortOrder = sortOrder.ToString();

        List<PersonResponse>? persons = _personsService.GetFilteredPersons(searchBy, searchString);
        List<PersonResponse>? sortedPerson = _personsService.GetSortedPerson(persons, sortBy, sortOrder);
        return View(sortedPerson);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem
        {
            Text = c.CountryName,
            Value = c.CountryId.ToString()
        });
        return View();
    }

    [HttpPost]
    public IActionResult Create(PersonAddRequest person)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.Erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View(person);
        }

        _personsService.AddPerson(person);
        TempData["SuccessMessage"] = "Record created successfully!";

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(Guid personId)
    {
        PersonResponse person = _personsService.GetPersonById(personId);

        if (person == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem
        {
            Text = c.CountryName,
            Value = c.CountryId.ToString()
        });
        ViewBag.Erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();


        return View(person.ToPersonUpdateRequest());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
    {
        PersonResponse? person = _personsService.GetPersonById(personUpdateRequest.PersonId);
        if (person == null)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Countries = _countriesService.GetAllCountries().Select(c => new SelectListItem
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            }).ToList();

            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            return View(personUpdateRequest);
        }

    }
    [HttpGet]
    public IActionResult Delete(Guid personId)
    {
        PersonResponse? person = _personsService.GetPersonById(personId);

        if (person == null)
        {
            return RedirectToAction("Index");
        }

        return View(person);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(PersonResponse personResponse)
    {
        bool isDeleted = _personsService.DeletePerson(personResponse.PersonId);
        if (isDeleted)
        {
            return RedirectToAction("Index");
        }
        ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        var person = _personsService.GetPersonById(personResponse.PersonId);
        return View(person);
    }
}