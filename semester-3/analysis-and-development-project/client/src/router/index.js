import { createRouter, createWebHistory } from 'vue-router';
import LiveMap from '../pages/LiveMap.vue';
import Plan from '../pages/PlanPage.vue';
import EmployeePage from '@/pages/EmployeePage.vue';
import BusinessPage from '@/pages/BusinessPage.vue';

const routes = [
  {
    path: '/',
    name: 'LiveMap',
    component: LiveMap,
  },
  {
    path: '/plan/:destinationId?',
    name: 'Plan',
    component: Plan,
  },
  {
    path: '/business',
    name: 'business',
    component: BusinessPage,
  },
  {
    path: '/employee',
    name: 'employee',
    component: EmployeePage,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
