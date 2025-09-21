# Secs
Ecs Library for CSharp - Fast, Fluent, Minimalistic 

> [!WARNING]
> This repository just a showcase that sparse-set ecs could be just as fast as archetype ones.
> Its not thread safe, its not covered with tests, the only tested cases is only benchmark cases.

## Benchmarks
> [!NOTE]
> Arch excluded from benchmark because its crashing on high amount
> of entities

### Iteration speed - lower is better

---
![img_1.png](img_1.png)

### Create entity and add components - lower is better

---
![img_2.png](img_2.png)

### Add component when iterating - lower is better

---
![img_3.png](img_3.png)

### Destroy when iterating - lower is better

---
![img_4.png](img_4.png)
