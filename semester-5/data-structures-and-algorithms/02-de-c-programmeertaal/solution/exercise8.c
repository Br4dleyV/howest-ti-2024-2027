#include <stdlib.h>
#include <stdio.h>

struct ll_element {
  int content;
  struct ll_element* next;
};

struct stack {
    struct ll_element* head;
};

struct stack* init() {
    struct stack *stack = malloc(sizeof(struct stack));
    stack->head = NULL;

    return stack;
}

void push(struct stack *stack, int value) {
    struct ll_element *el = malloc(sizeof(struct ll_element));
    el->content = value;
    el->next = NULL;

    if (stack->head != NULL) {
        // stack wasn't empty so push new element before existing elements
        el->next = stack->head;
    }
    stack->head  = el;
}

int pop(struct stack *stack) {
    if(stack->head == NULL) {
        fprintf(stderr, "Stack is empty!\n");
        exit(EXIT_FAILURE);
    }

    int value = stack->head->content;

    // remove the element from the stack
    struct ll_element *to_remove = stack->head;
    stack->head = to_remove->next;
    free(to_remove);

    return value;
}

void destroy(struct stack *stack) {
    // first free all the elements
    struct ll_element *el = stack->head;
    while(el != NULL) {
        struct ll_element *next = el->next;
        free(el);
        el = next;
    }
    // now free the stack container itself
    free(stack);
}

int main() {
    struct stack *stack = init();

    push(stack, 1);
    push(stack, 2);
    push(stack, 3);
    push(stack, 4);
    push(stack, 5);

    printf("%d\n", pop(stack));
    printf("%d\n", pop(stack));
    printf("%d\n", pop(stack));

    destroy(stack);
}
