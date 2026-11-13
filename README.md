# My Closet

A digital closet I made in C# with SplashKit for my OOP custom program.

You can browse your clothes by category, filter them, and put an outfit together on a scrapbook-style page. It also gives the outfit a colour-harmony score out of 100 and can suggest outfits where the colours go well together.

## Features

- Browse tops, bottoms, shoes and accessories, and see each one up close on a polaroid
- Filter each category (sleeve type, pants or skirts, shoe type, accessory type)
- **Surprise me:** picks a random outfit
- **Match colours:** suggests the best colour-matched outfits. If you're browsing an item, it keeps that item and matches everything else to it. Press again for the next suggestion
- Live harmony score with a short reason, e.g. `Harmony 82/100: neutral base, echo`
- Saves your look and loads it again next time

## Controls

| Key | What it does |
|---|---|
| S / B / H / A | Shirts / Bottoms / sHoes / Accessories |
| ← → | Browse the category |
| 1 | Filter on / off |
| ↑ ↓ | Change the filter |
| R | Surprise me (random outfit) |
| M | Match colours |
| Enter | Save the look |
| Esc | Back to the start screen |

The outfit also saves by itself whenever you switch category.

## Running it

You need the .NET SDK and SplashKit installed.

```bash
dotnet run
```

Run it from the project folder, so it can find `Data/` and `Resources/`.

## Adding your own clothes

Each item is one line in `Data/closet.txt`:

```
Type | Name | R,G,B colour | Picture | Extras

Accessory | Black Leather Cap  | 36,35,38    | Resources/images/accessories/black_leather_cap.png | Hat
Shirt     | Gingham Ribbon Top | 146,147,142 | Resources/images/tops/gingham_ribbon_top.png       | Short | yes
```

The extras depend on the type:

- **Shirt:** sleeve type, patterned (yes/no)
- **Pants / Skirt:** length, patterned (yes/no)
- **Shoes:** heel height, shoe type
- **Accessory:** accessory type

Lines starting with `#` are ignored. If a line has a mistake, it gets skipped with a message in the console, and the rest of the closet still loads.

## How it's built

```
Model/      ClothingItem and its 5 types, Closet, Outfit
Loading/    ClosetLoader reads the file, ClothingItemFactory builds each item
States/     Idle, Browse and Filter modes
Filters/    One class per filter
Harmony/    Colour rules, the harmony score and OutfitSuggester
Storage/    Saving and loading the outfit
UI/         Drawing the page and all the key bindings
```

Design patterns I used:

- **State:** each mode is its own class, so `ClosetApp` never checks which mode it's in
- **Strategy:** filters and colour rules are swappable classes behind one interface
- **Composite:** `CompositeRule` combines the 5 colour rules into one weighted score
- **Factory:** `ClothingItemFactory` turns a line of the file into the right clothing object
- **Template Method:** `ClothingItem.Display()` and `CategoryState` keep shared steps in one place
- **Repository:** `IOutfitStorage` hides how the outfit is saved

### Colour harmony

Each colour is turned into hue, saturation and lightness, then scored by 5 rules:

| Rule | Checks | Weight |
|---|---|---|
| Neutral base | at least half the pieces are neutral | 25% |
| Accent | one pop of colour | 20% |
| Contrast | enough light against dark | 20% |
| Undertone | warm and cool tones don't clash | 20% |
| Echo | a colour repeats somewhere | 15% |

A score of 70 or more gets a compliment. Below that, you get a tip from the weakest rule.

For **M**, `OutfitSuggester` scores every possible outfit and keeps the best 200 with a min-heap. Then it picks up to 10 that look different from each other.
