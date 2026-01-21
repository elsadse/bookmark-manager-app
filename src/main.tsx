import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import '@/index.css'
import { App } from '@/App'
import $ from 'jquery'
import _ from 'lodash'
import noUiSlider from 'nouislider'
import 'datatables.net'
import 'dropzone/dist/dropzone-min.js'
import * as VanillaCalendarPro from 'vanilla-calendar-pro'
import { BrowserRouter } from 'react-router'

window._ = _
window.$ = $
window.jQuery = $
window.DataTable = $.fn.dataTable
window.noUiSlider = noUiSlider
window.VanillaCalendarPro = VanillaCalendarPro

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
)
