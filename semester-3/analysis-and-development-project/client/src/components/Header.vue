<script setup>
import Clock from './Clock.vue';
import HamburgerSvg from '@/components/svgs/HamburgerSvg.vue';
import ClockSvg from '@/components/svgs/ClockSvg.vue';

const props = defineProps({
  onHamburgerClick: Function,
});
</script>

<template>
  <!-- Header -->
  <header>
    <!-- Hamburger icon and SkyNet title -->
    <div>
      <button @click="props.onHamburgerClick">
        <HamburgerSvg />
      </button>
      <a href="/"><h1>SKYNET</h1></a>
    </div>

    <!-- Current time display -->
    <div>
      <ClockSvg />
      <span><Clock /></span>
    </div>
  </header>
</template>

<style scoped>
/* Reset styling */
a {
  text-decoration: none;
  color: inherit;
}
</style>
