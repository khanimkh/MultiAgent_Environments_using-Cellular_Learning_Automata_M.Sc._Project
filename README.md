# Multi-Agent Rescue Task Allocation Using Cellular Learning Automata

## Overview

This project presents a distributed, learning-based approach for spatial–temporal task allocation in multi-agent rescue environments. It introduces a model where human rescuers collaborate with intelligent assistant agents, leveraging Cellular Learning Automata (CLA) and GIS data to optimize the assignment of dynamically discovered rescue tasks. The methodology is validated in a simulated urban disaster scenario, focusing on minimizing rescue time and travel distance while ensuring efficient coordination among teams.

**Key contributions:**
- Fully distributed, adaptive task allocation framework using CLA
- Integration of real-time geospatial data and agent-based simulation
- Mathematical modeling of agent learning and coordination
- Empirical evaluation against auction-based and threshold-based methods

## Introduction

Efficient task allocation in multi-agent environments (MAE) is a key challenge in distributed artificial intelligence. In urban search and rescue, coordination between team members is critical due to environmental complexity. This project proposes a new approach for allocating spatial–temporal tasks in multi-agent systems using a stochastic reinforcement learning algorithm based on cellular learning automata and GIS data.

## Problem Definition

The problem is formulated as a **Spatial–Temporal Coordination Problem (STCP)** in disaster response.

- **Search teams:** $S = \{S_1, S_2, \ldots, S_N\}$
- **Rescue teams:** $R = \{R_1, R_2, \ldots, R_M\}$
- **Rescue tasks:** $L = \{l_1, l_2, \ldots, l_K\}$

Each task is dynamically discovered, has spatial and temporal constraints, and must be assigned to a rescue team to minimize rescue time and travel distance, respecting team capabilities and avoiding negative impacts on neighboring teams.

## Dataset Resources

The study uses a geospatial simulation environment modeling realistic disaster scenarios, with inputs including map data, team locations, victim estimates, and task urgency levels. The case study is based in Habibabad city, Isfahan province, Iran.

## Preprocessing Steps

- Crisis area division
- Initial damage estimation
- Search task execution
- Data synchronization (local and central GIS databases)
- Task characterization (location, urgency, estimated time, deadline)

## Data and Environment Setup

- **Software:** C#/.NET, ArcGIS Desktop, DotSpatial, SQL Database
- **Components:** Model layer, scheduler, controller, GIS engine
- **Map scale:** 1:5000

## Model and Methodology

### Multi-Agent Architecture
Each search team is equipped with an intelligent assistant agent that detects tasks, selects rescue teams, learns from outcomes, and coordinates with neighbors.

#### Cellular Learning Automata (CLA)
- Each search team = one cell
- Each cell contains a Learning Automaton (LA)
- CLA enables local interactions, parallel learning, and decentralized coordination

**Learning Automaton:**
- Variable-structure stochastic automaton
- Linear Reward-ε-Penalty (LReP) scheme ($a = 0.4$, $b = 0.1$)
- Decision factors: distance, capability, workload, urgency, neighbor impact

## Mathematical Formula

### Response Threshold Model
$T_{ij}(s_j) = \frac{s_j^n}{s_j^n + \theta_{ij}^n}, \quad n > 1$

### Learning Automaton Update Rules
- **Reward:**
  $P_i(t+1) = P_i(t) + a(1 - P_i(t)$
  $P_j(t+1) = (1 - a)P_j(t), \quad j \neq i$
- **Penalty:**
  $P_i(t+1) = (1 - b)P_i(t)$
  $P_j(t+1) = \frac{b}{r-1} + (1 - b)P_j(t), \quad j \neq i$

**Neighbor-Aware Reward/Penalty:**
$\beta = T_1 \wedge T_2 \wedge T_3$

## Evaluation Metrics

- Average Rescue Time (seconds)
- Total Travel Distance (meters)
- Rescue Team Efficiency (%)
- Total System Runtime

## Results

- CLA outperforms auction-based and response-threshold methods
- Lowest average rescue time and shortest travel distance
- Better scalability and efficiency

## Conclusion

This work presents a distributed, learning-based framework for spatial–temporal task allocation in multi-agent rescue environments, combining human decision-making, intelligent assistant agents, and cellular learning automata. The approach achieves faster, more efficient, and more coordinated rescue operations without centralized control.

## References & Resources
- [Khani, M., Ahmadi, A. & Hajary, H. Distributed task allocation in multi-agent environments using cellular learning automata. Soft Comput 23, 1199–1218 (2019)](https://doi.org/10.1007/s00500-017-2839-5)
- [Khani, M., Ahmadi, A. & Khademi, M. A Model Based on Cellular Learning Automata for Improving the Intelligent Assistant Agents & Its Application in Earthquake Crisis Management. IJICTR. Volume 7 (2015)](https://ijict.itrc.ac.ir/article-1-107-en.html)
