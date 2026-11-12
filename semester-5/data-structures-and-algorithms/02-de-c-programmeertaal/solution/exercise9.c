#include <stdlib.h>
#include <stdio.h>

struct ll_element {
  int content;
  struct ll_element* next;
};

struct queue {
    struct ll_element* head;
    struct ll_element* tail;
};

struct queue* init() {
    struct queue *queue = malloc(sizeof(struct queue));
    queue->head = NULL;
    queue->tail = NULL;
    
    return queue;
}

void enqueue(struct queue *queue, int value) {
    struct ll_element *el = malloc(sizeof(struct ll_element));
    el->content = value;
    el->next = NULL;

    if(queue->head == NULL) {
        // first element added to the queue becomes head
        queue->head = el;
    } else {
        // add at the end of the queue
        queue->tail->next = el;
    }
    queue->tail = el;
}

int dequeue(struct queue *queue) {
    if(queue->head == NULL) {
        fprintf(stderr, "Queue is empty!\n");
        exit(EXIT_FAILURE);
    }

    int value = queue->head->content;

    // remove first element from the queue
    struct ll_element *to_remove = queue->head;
    queue->head = to_remove->next;
    free(to_remove);

    // also erase tail if queue became empty
    if(queue->head == NULL) {
        queue->tail = NULL;
    }

    return value;
}

void destroy(struct queue *queue) {
    // first free all the elements
    struct ll_element *el = queue->head;
    while(el != NULL) {
        struct ll_element *next = el->next;
        free(el);
        el = next;
    }
    // now free the queue container itself
    free(queue);
}

int main() {
    struct queue *queue = init();

    enqueue(queue, 1);
    enqueue(queue, 2);
    enqueue(queue, 3);
    enqueue(queue, 4);
    enqueue(queue, 5);

    printf("%d\n", dequeue(queue));
    printf("%d\n", dequeue(queue));
    printf("%d\n", dequeue(queue));

    destroy(queue);
}
