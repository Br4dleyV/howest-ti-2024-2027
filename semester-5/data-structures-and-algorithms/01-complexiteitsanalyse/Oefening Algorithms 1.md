# Algoritmes oefening 2

Bepaal de asymptotische tijdscomplexiteit $\cal{O}(f(n))$ van onderstaande algoritmes. Definieer ook duidelijk in functie van welke waarde of grootheid je de tijdscomplexiteit specifieert.

## 1. Gemiddelde berekenen

```python
def mean(values):
    total = 0

    for value in values:
        total += value

    return total / len(values)
```

n is de lengte van de values
1 lus dus O(n) dus lineair

De complexiteit is O(n) lineair omdat de code 1 keer over alle elementen loopt.

## 2. Controleren of 2 lijsten identiek zijn

```python
def identical(a, b):
    if len(a) != len(b):
        return False

    for i in range(len(a)):
        if a[i] != b[i]:
            return False

    return True
```

n is de lengte van a
1 lus dus O(n) dus lineair

De complexiteit is O(n) lineair omdat de code 1 keer over alle elementen loopt.

## 3. Mediaan van gesorteerde lijst berekenen

```python
def median(values):
    n = len(values)

    if n % 2 == 1:
        return values[n // 2]

    return (values[n // 2 - 1] + values[n // 2]) / 2
```

n is lengte van values
1 resultaat dus O(1) dus constant

De complexiteit is O(1) omdat er maar 1 resultaat wordt teruggegeven, er is geen loop.

## 4. Fibonacci (met loop)

```python
def fibonacci(n):
    a = 0
    b = 1

    for i in range(n):
        next_value = a + b
        a = b
        b = next_value

    return a
```

n is invoerwaarde
1 lus dus O(n) dus lineair

## 5. Fibonacci (recursief)

```python
def fibonacci(n):
    if n <= 1:
        return n

    return fibonacci(n - 1) + fibonacci(n - 2)
```

n is invoerwaarde
elke functie voert opnieuw 2 functies uit, dus exponentieel
O(2^n)

## 6. Element zoeken in gesorteerde lijst (recursief)

```python
def binary_search(values, target):
    if len(values) == 0:
        return False

    middle = len(values) // 2

    if values[middle] == target:
        return True

    if target < values[middle]:
        return binary_search(values[:middle], target)

    return binary_search(values[middle + 1:], target)
```

n is lengte van values
elke stap halveert de lijst, dus O(log n)

De complexiteit is O(log n) omdat de invoer elke stap halveert.

## 7. Controleren op duplicaten in lijst

```python
def has_duplicates(values):
    for i in range(len(values)):
        for j in range(i + 1, len(values)):
            if values[i] == values[j]:
                return True

    return False
```

n is de lengte van values
er zijn twee geneste loops
dus O(n^2)

De complexiteit is O(n^2) omdat er twee nested loops zijn.

## 8. Twee gesorteerde lijsten samenvoegen

```python
def merge(a, b):
    result = []
    i = 0
    j = 0

    while i < len(a) and j < len(b):
        if a[i] < b[j]:
            result.append(a[i])
            i += 1
        else:
            result.append(b[j])
            j += 1

    while i < len(a):
        result.append(a[i])
        i += 1

    while j < len(b):
        result.append(b[j])
        j += 1

    return result
```

n is de lengte van lijst a, m is de lengte van lijst b
alle elementen uit beide lijsten worden 1 keer bekeken
O(n + m)

## 9. Grootste gemene deler bepalen

```python
def gcd(a, b):
    while b != 0:
        remainder = a % b
        a = b
        b = remainder

    return a
```

het resultaat wordt telkens kleiner
O(log n) dus logaritmisch

## 10. Controleren of een getal een priemgetal is

```python
def is_prime(n):
    if n < 2:
        return False

    divisor = 2

    while divisor * divisor <= n:
        if n % divisor == 0:
            return False

        divisor += 1

    return True
```

O(vkw(n))

De complexiteit is O(vkw(n)) omdat het tot de wortel van n controleert.

## 11. Twee vierkante matrices vermenigvuldigen

```python
def multiply(a, b):
    n = len(a)
    result = []

    for i in range(n):
        row = []

        for j in range(n):
            value = 0

            for k in range(n):
                value += a[i][k] * b[k][j]

            row.append(value)

        result.append(row)

    return result
```

3 nested loops
O(n^3) kubisch

## 12. Naive Substring Search

```python
def contains(text, pattern):
    for i in range(len(text) - len(pattern) + 1):
        match = True

        for j in range(len(pattern)):
            if text[i + j] != pattern[j]:
                match = False
                break

        if match:
            return True

    return False
```

2 nested loops
O(n^2) kwadratisch