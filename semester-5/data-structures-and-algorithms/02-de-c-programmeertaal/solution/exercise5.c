#include <stdlib.h>

void fill(int *data, int length, int content) 
{
    for (int i = 0; i < length; i++)
    {
        data[i] = content;
    }
}

int main() 
{
    int length = 4;
    int content = 2;

    int *data_array = malloc(length*sizeof(int));

    fill(data_array, length, content);

    free(data_array);
}
