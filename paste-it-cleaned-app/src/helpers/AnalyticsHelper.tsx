import moment from 'moment'
import React from 'react'

import { WordIcon } from '../icons/Word'
import { ExcelIcon } from '../icons/Excel'
import { PowerPointIcon } from '../icons/PowerPoint'
import { WebIcon } from '../icons/Web'
import { TextIcon } from '../icons/Text'
import { ImageIcon } from '../icons/Image'

export function createData(timeStamp: Date, ip: string, type: string, userAgent: string, price: number) {
    var timeStampNode = moment(timeStamp).format('YYYY-MM-DD HH:MM:SS')
    var ipNode = (
        <a target="_blank" href={`https://iplogger.org/ip-lookup/?d=${ip}`}>
            {ip}
        </a>
    )
    var userAgentNode = userAgent
    var priceNode = price > 0 ? '$' + price.toString() : '-'
    var typeNode = <span className="hit-type">?</span>

    switch (type.toLowerCase()) {
        case 'word':
            typeNode = (
                <span className="hit-type">
                    <WordIcon /> Word
                </span>
            )
            break
        case 'excel':
            typeNode = (
                <span className="hit-type">
                    <ExcelIcon /> Excel
                </span>
            )
            break
        case 'powerpoint':
            typeNode = (
                <span className="hit-type">
                    <PowerPointIcon /> PowerPoint
                </span>
            )
            break
        case 'web':
            typeNode = (
                <span className="hit-type">
                    <WebIcon /> Web
                </span>
            )
            break
        case 'text':
            typeNode = (
                <span className="hit-type">
                    <TextIcon /> Text
                </span>
            )
            break
        case 'image':
            typeNode = (
                <span className="hit-type">
                    <ImageIcon /> Image
                </span>
            )
            break
    }

    return { key: '1', timeStamp: timeStampNode, ip: ipNode, type: typeNode, userAgent: userAgentNode, price: priceNode }
}
