import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'

import RoutedApp from './RoutedApp'

import './index.less'

ReactDOM.render(
    <BrowserRouter>
        <RoutedApp />
    </BrowserRouter>,
    document.getElementById('root')
)
