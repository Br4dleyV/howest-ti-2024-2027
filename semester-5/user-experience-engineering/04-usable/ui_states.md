# UI states
https://www.scotthurff.com/posts/why-your-user-interface-is-awkward-youre-ignoring-the-ui-stack

## 1. What is each state used for?

- Ideal state: This shows the product delivering its intended value with useful content available
- Empty state: Explains why there is no content and helps the user take the next step. This can happen onfirst use, after clearing content, or when a search returns nothing
- Error state: Explains a problem and how to recover, while preserving the user's work
- Partial state: Handles limited content or incomplete progress and guides the user towards a more useful experience
- Loading state: Communicates that something is happening while the user waits for content or an action to finish

## 2. Good and bad examples

Good and bad example for:
- Ideal state: 
    - Good: Tinder's populated match interface demonstrates the product's core value: finding people to connect with
    - Bad: A populated recipe page hides the ingredients behind several promotional panels, making the main task difficult even when all content is available
- Empty state: 
    - Good: Airbnb's empty Wish List gives users a clear action to start collecting places
    - Bad: A new budgeting app shows only “No data,” leaving the user unsure whether to add an account or enter an expense
- Error state: 
    - Good: In Hurff's messaging prototype, an unsuccessful message remains visible and can be retried
    - Bad: A booking form rejects a postcode and clears every field, forcing the user to re-enter their details
- Partial state: 
    - Good: LinkedIn's profile completeness indicator suggests specific steps for completing a profile
    - Bad: A dashboard with one recorded workout displays an unexplained flat trend line, suggesting a meaningful trend without enough data
- Loading state: 
    - Good: Polar's skeleton screens show the page structure while content arrives
    - Bad: A file upload shows an endlessly spinning icon without progress, a cancel option, or an explanation when the connection fails

